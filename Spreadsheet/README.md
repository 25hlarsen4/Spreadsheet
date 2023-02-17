```
Author:			Hannah Larsen
Partner:		None
Course:			CS3500, University of Utah, School of Computing
GitHubID:		25hlarsen4
Repo:			https://github.com/uofu-cs3500-spring23/spreadsheet-25hlarsen4
Date:			16-Feb-2023 6:05 pm
Project:	  	Spreadsheet
Copyright:		CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
```


# Comments to Evaluators:

In every method involved writing or reading Xml files, after writing a catch block for all specific exceptions 
that the methods were expected to throw, like InvalidNameException, FormulaFormatException, etc, because it was 
recommended by a TA, I added another catch block at the end to catch any other exceptions that may arise with a 
more vague message, "other errors". While I was able to test that the methods threw all the specific exceptions 
that were expected, I could not test the "other errors" catch block, which resulted in a slightly lower code coverage
percentage. However, since we can rely on the fact that C# catch blocks work, I can trust that it will act as expected.

# Assignment Specific Topics:

No additional write up was required by this assignment.

# Consulted Peers:

I consulted with one of my lab peers, Todd, and we talked about good references we had found to help
us with the xml reading and writing portions of the assignment.

# References:

1. XmlWriter Class - https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlwriter?view=net-7.0
2. XmlReader.Read Method - https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.read?view=net-7.0