# VIPR: Client Library Generation Toolkit

[![Build status][build-status-image]][build-status]  [![Issue Stats][pull-requests-image]][pull-requests]  [![Issue Stats][issues-closed-image]][issues-closed]

[build-status-image]: http://corefx-ci.cloudapp.net/jenkins/job/microsoft_vipr/badge/icon
[build-status]: http://corefx-ci.cloudapp.net/jenkins/job/microsoft_vipr/
[pull-requests-image]: http://www.issuestats.com/github/microsoft/vipr/badge/pr
[pull-requests]: http://www.issuestats.com/github/microsoft/vipr
[issues-closed-image]: http://www.issuestats.com/github/microsoft/vipr/badge/issue
[issues-closed]: http://www.issuestats.com/github/microsoft/vipr

VIPR is an extensable toolkit for generating Web Service Client Libraries. VIPR is 
designed to be highly extensible, enabling developers to adapt it to read new Web
Service description languages and to create libraries for new target platforms with ease.

This repository contains the core VIPR infrastructure, Readers for OData v3 and v4, and
Writers for C#, Objective-C, and Java. It also contains a Windows Command Line Interface
application that can be used to drive Client Library generation.

Today, Vipr is composed of the following components:

* **Vipr.Core**. Provides the interfaces required for extending Vipr as well as the core Client Library
  generation logic. It also defines the ODCM Object Model used to describe service capabilities between
  Readers and Writers.

* **Vipr**. Command Line Interface enabling generation of Client Libraries on Windows.

* **ODataReader.v3**. IOdcmReader implementation for converting OData v3 metadata into an OdcmModel.

* **ODataReader.v4**. IOdcmReader implementation for converting OData v4 metadata into an OdcmModel.

* **CSharpWriter**. IOdcmWriter implementation for converting an OdcmModel into a C# Client Library.

* **TemplateWriter**. IOdcmWriter implementation for converting an OdcmModel into a Java or Objective-C Client Library.

## How to Engage, Contribute and Provide Feedback

Some of the best ways to contribute are to try things out, file bugs, and join in design conversations. 

Want to get more familiar with what's going on in the code?
* [Pull requests](https://github.com/microsoft/vipr/pulls): [Open](https://github.com/microsoft/vipr/pulls?q=is%3Aopen+is%3Apr)/[Closed](https://github.com/microsoft/vipr/pulls?q=is%3Apr+is%3Aclosed)

Looking for something to work on? The list of [up-for-grabs issues](https://github.com/microsoft/vipr/issues?q=is%3Aopen+is%3Aissue+label%3Aup-for-grabs) is a great place to start.

* [How to Contribute][Contributing Guide]
    * [Contributing Guide][Contributing Guide]
    * [Developer Guide]

You are also encouraged to start a discussion by filing an issue or creating a
gist. See the [contributing guides][Contributing Guide] for more details. 

[Contributing Guide]: https://github.com/microsoft/vipr/wiki/Contributing
[Developer Guide]: https://github.com/microsoft/vipr/wiki/Developer-Guide

## License

This project is licensed under the [MIT license](LICENSE).