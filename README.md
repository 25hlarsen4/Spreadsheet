```
Author:				Hannah Larsen
Partner:			Todd Oldham
StartDate:			13-Jan-2023
Course:				CS3500, University of Utah, School of Computing
GitHubID:			25hlarsen4, Destroyr-of-u
Repo:				https://github.com/uofu-cs3500-spring23/assignment-six---gui---functioning-spreadsheet-larsenoldham.git
CommitDate:			02-March-2023, 2:40 pm
Solution:			Spreadsheet
Copyright:			CS3500, Hannah Larsen, Todd Oldham - This work may not be copied for use in academic coursework.
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

As of the completion of Assignment 4, we have started the internals of an actual spreadsheet program. More specifically,
we have a working representation of a spreadsheet that can set cell contents and keep track of dependencies between cells, 
using the DependencyGraph created in Assignment 2.

Now, as of the completion of Assignment 5, we have furthered the internal of our spread program. More specifically, we have
added functionality to get and set cell values, rather than just contents, and have added functionality to create xml files
representing spreadsheets.

With the completion of assignment six the spreadsheet will now have a GUI that the users can use to change, save, and open
spreadsheet files.

# Examples of Good Software Practices:

First, my use of DRY programming shows good software practices. I used DRY programming by using inheritdoc rather than explicitly
repeating method headers that were already written in the AbstractSpreadsheet class.
Furthermore, I used code re-use to show good software practice. I specifically did this by creating a helper method to use in 
each of the three SetCellContents methods since each of those methods included a lot of the same code.
See the following list for more good software practices that I used.
Finally, my use of well named methods shows good software practices. You can specifically see this in methods like
DetermineIfNameIsInvalid, RecalculateCellValues, DetermineVariableValidity, etc, all of which have names that clearly
describe what they accomplish.

- Separation of concerns
- Well named methods

# Best Team Practices:
The partnership was most effective in that we communicated consistently and effectively so we could meet up several times early 
in the week to get the assignment done. Furthermore, when we weren't actively working together, we separately researched issues 
that had arisen so our coding process could be faster when we met up again later to fix the issue. Almost the assignment was done
by pair programming standards together, so we did not have to assign each other small tasks to complete since we always worked on
the assignment together.

An area of teamwork that we could improve upon is if we hit a problem that neither of us can solve, instead of wasting many hours
on trying to figure it out in one sitting, we should try to take a break and reach out for outside guidance in the future. Other 
than that, we worked together well and had no other issues.

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

Assignment 5 - A Complete Spreadsheet Model:		Predicted hours: 12		   Actual hours: 13\
Note: Two hours were spent debugging my strategy to detect cycles, but other than that my time was evenly
distributed amongst the assignment tasks.

Assignment 6 - Spreadsheet Front-End Graphical User Interface:		Predicted hours: 12		   Actual hours: 12\
Note: We each spent those 12 hours working together via pair programming.
Also note that several hours were inefficiently spent trying to understand the basics of how to set up the spreadsheet 
layout, but other than that, our time was spent evenly across the assignment requirements.

Both of our time estimation skills are improving throughout this semester. In this first assignment working together, 
we were able to correctly guess how long it would take us. This tells us that we have an accurate perception of our own 
programming abilities and that each of our abilities line up fairly well.
