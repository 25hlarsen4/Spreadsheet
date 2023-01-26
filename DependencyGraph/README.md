```
Author:			Hannah Larsen
Partner:		None
Course:			CS3500, University of Utah, School of Computing
GitHub ID:		25hlarsen4
Repo:			https://github.com/uofu-cs3500-spring23/spreadsheet-25hlarsen4
Date:			25-Jan-2023 11:30 PM
Project:	  	DependencyGraph
Copyright:		CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
```


# Comments to Evaluators:

I have no comments at this time as my work currently stands on its own.

# Assignment Specific Topics:

No additional write up was required by this assignment.

# Consulted Peers:

I did not consult with any peers on this project because the only issue I was having significant trouble 
resolving was how to achieve constant time complexity in all the cases I needed to since although I chose 
to use dictionaries which have constant time complexity for looking up keys, I originally used LinkedLists 
as the values which did not have constant time complexity for removal. However, through helpful hints in 
lecture, I thought of using HashSets instead of LinkedLists to achieve the constant time complexity I needed,
therefore I did not end up consulting with peers on this issue.

# References:

1. Dictionary<TKey, TValue> Class - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=net-7.0
2. HashSet<T> Class - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-7.0