# Extended Property Language

The Tingler is a metadata compiler, where data is treated like an object model patterned after collections of key value property.
The successfully artifact is a package that can be deployed for a specific environment.

The current targeted environment is Azure Data Factory.  ADF requires the definition of activities, data sets, authorizations, etc.. in
json format.  This tool provides the language centric capabilities of defining ADF pipelines with artifact support for activities
that require it.

Primary processes are...

1. Compile Tingler language into intermediate language (IL).
2. Linker for building a package that includes IL and required artifacts.
3. Deploy package to specific environment
4. Start / stop a ADF pipeline manually

<br>

Source file are normal text files with required instructions with the default extension of ".pc".
Sources, IL, and artifacts are packaged into zip archives with the default extension of ".pcPackage".
A package has the complete set of information to build ADF pipelines and deploy artifacts required by its activities.


#### Key words

* assembly
* include
* resource
* readonly

<br>

### Scalar


> variable = value;

Currently only strings are supported.

<br>

### Object

> \{**objectName**} = scalarValue;
> 
> \{**objectName**} = { \{**propertyAssignment**}[, \{**propertyAssignment**}, ...] };
> 
> \{**objectName**} = objectName { \{**propertyAssignment**}[, \{**propertyAssignment**}] };

##### Expression

>\{**objectName**} : [readonly] objectName;
>
>\{**propertyAssignment**} : [readonly] name = scalarValue | objectName;


Create an object with property(s) and support inheritance from other objects.

Some examples are...

```
firstName = "Jeff Roberts";

fullName = { First = "Jeff", Last = "Roberts" }

address = {
    street = "One street",
    city = "City",
    State = "state",
    Zip = "zipCode"
}

profile = {
    firstName = firstName,
    fullName = fullName with {
        middleInitial = "M"
    },
    address = address with {
        zipCode = "98033"
    }
}

```

<br>

### Assembly Reference

> assembly file;

Load assembly for use in package.

<br>

### Include

> include file;

Include another source file in the complication process at the point in the code.  Normally used to defined
base object used like templates.

<br>

### Resource

> resource resourceId = directory | file;

Assign resourceId to a one or more files.  These references are normally included in building the package.

<br>
