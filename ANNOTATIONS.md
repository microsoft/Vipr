# Annotations support in VIPR

Annotations support in VIPR is based on the work originally done in the Annotations branch. This branch has been since 
merged into VIPR master (commit XXX).

The following block is a brief description taken from one of Annotations branch commits.

***
*Support for Capability Annotations Reading in OdcmModel*

We are starting with a limited set of Capabilities - InsertRestriction, UpdateRestriction, DeleteRestriction and ExpandRestriction.
Capabilities define how an EntityType or its collection is projected in the generated code.
To enable that OdcmModel introduces a new class called OdcmProjection.
This class contains the OdcmType (EntityType to be projected) and the capabilities (the operations to be projected for the EntityType).
On the other hand each OdcmType will maintain a list of corresponding OdcmProjections.
The OdcmWriters will use this projection information to drive the codegen of interfaces (or similar artifacts) for a given OdcmType.
The emitted interfaces will only project those capabilities that are available when accessing an EntityType (or its collection) from a given context.

In the OdcmModel, both Entity Sets (of an EntityContainer) and Navigation Properties (of an EntityType) are both modeled into OdcmProperty.
Given that the Capabilities we are supporting will affect Client Library properties which give access to Entities and Entity sets, the OdcmProperty class will be updated to expose a Projection property. OdcmProjection is added as a property to the OdcmProperty class, so each OdcmProperty is mapped to an OdcmProjection.  [BreakingChange] Also the OdcmType property of the OdcmProperty class is now redundant and will be removed [/BreakingChange].

Essentially there are three major changes in the OdcmModel.
- OdcmType will have a list of OdcmProjections. The OdcmWriter will generate interfaces for each Projection of an OdcmType.
- OdcmProperty will have a reference to an OdcmProjection. OdcmWriter will resolve this Projection to one of the interfaces generated in the above step. That resolved interface will be used as the return type of the Property when codegening.
- OdcmProperty class will no longer have OdcmType property.
***

Current VIPR version derives from this work and significantly extends it. The main goal was to make annotation support much more general.

Since it's not easy to draw the line between capability and non-capability annotations, it makes sense to talk about supporting annotations in general.
Basically, OdcmModel should somehow represent a set of annotations for an arbitrary object. OdcmWriter should have a way to query annotations associated
with a specific object. It is responsibility of OdcmWriter to interpret those annotations.

The Description and LongDescription annotations are already supported in VIPR MASTER. The rest of annotations defined by Core and Capability
vocabularies can be roughly classified as follows:

- Boolean constants
- String constants
- Enumerations
- Collections/lists
- Records/complex types (i.e. composites)

Collections can contain constants or records; records can contain collections.

## Support for capability annotations in VIPR MASTER

VIPR MASTER only has support for 4 annotations with very similar structure ("restriction" annotations):

- Capabilities.UpdateRestrictionType
- Capabilities.DeleteRestrictionType
- Capabilities.ExpandRestrictionType
- Capabilities.InsertRestrictionType

All of these types are defined as a record that consists of boolean property (e.g. Updatable) and of a "negative" collection of navigation
properties (e.g. NonUpdatableNavigationProperties).

In Odcm model, these annotations are represented by objects derived from OdcmBooleanCapability class. For instance, UpdateRestrictionType 
will be converted into OdcmUpdateCapability object with Updatable value plus a set of OdcmUpdateLinkCapability objects with "False" value for each of
NonUpdatableNavigationProperties. Similarly, there is OdcmDeleCapability and OdcmDeleteLinkCapability classes. For ExpandRestrictionType and
InsertRestrictionTypes, only OdcmExpandCapability and InsertCapability classes are defined (i.e. "link" capabilities are missing). That gives 6 
capabilities in all.

Note that in VIPR MASTER only parses capabilty annotations defined on EntitySet elements.

The list of 6 predefined capabilities is called a Projection and represented by OdcmProjection class. Each annotationable object in Odcm model has 
a projection reference. If any of the 6 capabilities is not defined for the object, its default value is used.

The Odcm writer code can check for any capability value using one of OdcmProjection methods, e.g. SupportsUpdate() or SupportsUpdateLink().

In addition to projections on individual objects, each object type (represented by OdcmType class) has a list of all distinct projections defined for
objects of this type. This list is used by Vipr.Writer.CSharp.Lite as follows: 
- the set of capabilities with TRUE values is used to generate interface names; that means there might be several interface versions for any type. 
Name generation uses short versions of capability names. For instance, an interface that supports all 6 capabilities will have suffix
"Del_Dlk_Exp_Ins_Ulk_Upd", while an interface that doesn't support neither DeleteLink nor UpdateLink capabilities, will have suffix 
"Del_Exp_Ins_Upd".
- the set of capabilities with TRUE values is used to generate methods available in each interface, for instance, an interface supporting DeleteLink
capability can have DeleteLinkAsync method defined. 

## Support for annotations in the updated VIPR (VIPR NEW below)

VIPR NEW extends the annotation-related functionality while keeping the code backwards-compatible. All unit tests existing in VIPR MASTER still pass.

VIPR NEW changes annotation support as follows:
- allows EDMX annotation parsing on arbitrary element (not just on EntitySet)
- handles wide variety of annotation types (not just restriction ones); all annotations types defined in Core or Capabilities vocabularies
should be supported. It should also be possible to define custom annotations.
- allows annotation "inheritance", e.g. EntitySet would inherit annotations defined by its EntityType.

Odcm model in VIPR NEW no longer provides the fixed set of capabilities in object projection; object
projections only refer to those annotations that were explicitly specified.
  
The name of annotation term is used as annotation identifier. 

To get the value of annotation defined as a boolean constant (e.g. Capabilities.TopSupported), Odcm writer 
would use this OdcmProjection method:

    bool? value = projection.Supports(annotationTerm);

Supports() method is equivalent to: 

    bool? value = projection.BooleanValueOf(annotationTerm);

BooleanValueOf() method returns null if this annotation is not explicitly defined and no default is provided.
The same is true for all XXXValueOf methods.

All restriction capabilities from VIPR MASTER are considered "well-known" and always have defaults.

Old VIPR MASTER methods (e.g. SupportsUpdate) are still available (and implemented via Supports):

    bool value = projection.SupportsUpdate();

OdcmProjection.UserDefaultCapabilityProvider property can be used as a generic defaults provider.

To get the value of annotation defined as a string constant (e.g. Core.ResourcePath) :

    string value = projection.StringValueOf(annotationTerm);

To get the value of annotation defined as an enumeration (e.g. Capabilities.ConformanceLevelType) :

    IEnumerable<string> value = projection.EnumValueOf(annotationTerm);

EnumValueOf method returns an IEnumerable of specified member names.

To get the value of annotation defined as a string collection (e.g. Capabilities.AcceptableEncodings) :

    IEnumerable<string> value = projection.StringCollectionValueOf(annotationTerm);

To get the value of annotation defined as complex type (e.g. Capabilities.CallbackType) :

    dynamic value = projection.RecordValueOf(annotationTerm);

After getting the dynamic value, it should be possible to get properties of the record, e.g. for
Capabilities.CallbackType:

    IEnumerable<dynamic> protocols = record.CallbackProtocols;
    string id = protocols.First().Id;
    
To get the value of annotation defined as a collection of arbitrary records:

    IEnumerable<dynamic> value = projection.CollectionValueOf(annotationTerm);
      
### Special cases of annotation representation in Odcm model

Some annotations are defined in Edmx as records, however their representation in Odcm model is different;
RecordValueOf should not be used for those types. Namely, UpdateRestrictionType, ExpandRestrictionType,
DeleteRestrictionType and InsertRestrictionType annotations
are represented exactly the same as in VIPR MASTER;
you can use BooleanValueOf for projections on EntitySet and on its navigation properties:

    bool isLinkUpdatable = navProperty.Projection.BooleanValueOf("NonUpdatableNavigationProperties");
    
or (for backwards compatibility)

    bool isLinkUpdatable = navProperty.Projection.SupportsUpdateLink();

For navigation properties in this case you can also use IsOneOf() method, for instance:

    bool isLinkUpdatable = !navProperty.Projection.IsOneOf("NonUpdatableNavigationProperties");
    
Other standard "restriction" types are handled in a very similar way. For instance, FilterRestrictionType would result in 2 boolean
capabilities for annotated EntitySet ("Filterable" and "RequiresFilter") and one of 2 boolean capabilities for its annotated properties
("RequiredProperties" and "NonFilterableProperties").

    bool requiresFilter = projection.BooleanValueOf("RequiresFilter");
    bool required = projection.IsOneOf("RequiredProperties");
    
### Annotation "inheritance"

The annotation applicable to EntitySet can be defined on its EntityType; this seems to be pretty common case (see, for instance,
Microsoft Graph metadata). VIPR NEW supports this.

### Projections cache    

VIPR NEW still supports the cache of projections on object type, however it does this just for 6 "well-known" boolean capabilities.


  




















   



