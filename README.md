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
spreadsheet files. To save, provide an absolute path, including what you want the file to be called without an extension 
(ie C:\Users\hanna\Downloads\Spreadsheets\blah). To open, provide the same thing that you typed in to save.

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

# How to use:
To fill a cell with an integer or string value, click the cell, type the value, and hit enter.
To fill a cell with a formula to be evaluated, for example to make cell C1 = A1 + B1, click cell C1 and type the desired formula, starting with an = sign, no spacing requirements.
To save, provide an absolute path, including what you want the file to be called without an extension (ie C:\Users\hanna\Downloads\Spreadsheets\blah). 
To open, provide the same thing that you typed in to save.
