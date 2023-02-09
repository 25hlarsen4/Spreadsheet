```
Author:			Hannah Larsen
Partner:		None
Course:			CS3500, University of Utah, School of Computing
GitHubID:		25hlarsen4
Repo:			https://github.com/uofu-cs3500-spring23/spreadsheet-25hlarsen4
Date:			08-Feb-2023 6:15 pm
Project:	  	Spreadsheet
Copyright:		CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
```


# Comments to Evaluators:

I was not able to test that SetCellContents(string name, Formula formula) throws an ArgumentNullException when
the formula parameter is null because it instead threw a NullReferenceException since Formula is non nullable, 
however I still checked for that case in my code since we were told to in the instructions.

# Assignment Specific Topics:

No additional write up was required by this assignment.

# Consulted Peers:

I consulted with one of my lab peers, Todd, and we talked about how we both were experiencing the issue noted in
the above Comments to Evaluators section.

# References:

1. HashSet<T> Class - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-7.0