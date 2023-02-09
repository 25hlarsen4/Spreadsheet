```
Author:				Hannah Larsen
Partner:			None
StartDate:			13-Jan-2023
Course:				CS3500, University of Utah, School of Computing
GitHubID:			25hlarsen4
Repo:				https://github.com/uofu-cs3500-spring23/spreadsheet-25hlarsen4
CommitDate:			09-Feb-2023 2:45 pm
Solution:			Spreadsheet
Copyright:			CS3500 and Hannah Larsen - This work may not be copied for use in academic coursework.
```


# Overview of the Spreadsheet Functionality:

As of the completion of Assignment 1, the Spreadsheet program was capable of evaluating valid mathematical 
expressions without negative integers, and the only operators allowed were +, -, *, /, (, and ).
Variables were also allowed (in the form of one or more upper or lower case letters followed by one or more digits), 
and their values were looked up using a delegate. 

As of the completion of Assignment 2, the Spreadsheet program had the capability to make dependency graphs to represent 
the dependencies between cells in a spreadsheet.

As of the completion of Assignment 3, a more generalized version of the evaluating functionality created in 
Assignment 1 has been achieved. There is now a Formula class with an Evaluate method within it, and it has been updated 
to work on doubles. Furthermore, several methods have been implemented to work on Formula objects.
Future extensions are still unclear, but the eventual goal is to have a spreadsheet that can keep track of dependencies
between cells to then be able to accurately compute and update formulas in the cells.

Now, as of the completion of Assignment 4, we have started the internals of an actual spreadsheet program. More specifically,
we have a working representation of a spreadsheet that can set cell contents and keep track of dependencies between cells, 
using the DependencyGraph created in Assignment 2.

# Examples of Good Software Practices:

First, my use of DRY programming shows good software practices. I used DRY programming by using inheritdoc rather than explicitly
repeating method headers that were already written in the AbstractSpreadsheet class.
Furthermore, I used the idea of separation of concerns to show good software practice. Specifically, I did this by not yet 
worrying about representing the values of cells. Although every cell will eventually have a value, the methods we implemented 
only concerned the cells' contents, so I did not yet account for cells' values in the Cell class.
Finally, I used code re-use to show good software practice. I specifically did this by creating a helper method to use in 
each of the three SetCellContents methods since each of those methods included a lot of the same code.
See the following list for more good software practices that I used.

- Short methods
- Well named methods

# Time Expenditures:

Assignment 1 - Formula Evaluator:   	Predicted hours: 12  	   Actual hours: 11\
Note: No serious hangups arose, so my time spent working was evenly distributed amongst the assignment tasks.

Assignment 2 - Dependency Graph:		Predicted hours: 13		   Actual hours: 12\
Note: No serious hangups arose, so my time spent working was evenly distributed amongst the assignment tasks.

Assignment 3 - Refactoring the FormulaEvaluator:		Predicted hours: 12		   Actual hours: 12\
Note: No serious hangups arose, so my time spent working was evenly distributed amongst the assignment tasks.

Assignment 4 - Onward to a Spreadsheet:		Predicted hours: 12		   Actual hours: 11\
Note: An hour was spent trying to fix a slightly off Regex expression, but other than that my time was evenly
distributed amongst the assignment tasks.
