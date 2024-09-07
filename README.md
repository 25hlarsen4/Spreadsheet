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

- Spreadsheet cells are capable of holding whole numbers, decimals, or string values.
- Spreadsheet cells are capable of performing mathematical formula evaluations. The allowed operators are +, -, *, /, (, and ).
  The allowed operands are typed out non-negative numbers or references to other cells which must have number values.
  Formula examples: =1+1 is valid, =-1+1 is not valid, =1-1 is valid.
                    Then say cell A1 contains value -1, a forumla referencing A1 like so is valid: =A1+1

- When you click on a cell/are focues on it, it will show the cell contents (either a typed out number or string value, or a formula that was entered)
- When you are no longer focused on a cell, it will show the actual cell value (either a typed out number or string value, or the evaluation of a formula in the cell)

As of the completion of Assignment 1, the Spreadsheet program was capable of evaluating valid mathematical 
expressions where the only operators allowed were +, -, *, /, (, and ).
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
(ie C:\Users\name\Downloads\Spreadsheets\nameOfSpreadsheetFile). To open, provide the same thing that you typed in to save.

# How to use:
- To fill a cell with an numerical or string value, click the cell, type the value, and hit enter.
- To fill a cell with a formula to be evaluated, for example to make cell C1 = A1 + B1, click cell C1 and type the desired formula, starting with an = sign, no spacing requirements.
  So for this example, click cell C1, type =A1+B1, and hit enter
- To save, provide an absolute path, including what you want the file to be called without an extension (ie C:\Users\name\Downloads\Spreadsheets\nameOfSpreadsheetFile). 
- To open a previously saved spreadsheet, provide the same path that you provided to save the spreadsheet.
