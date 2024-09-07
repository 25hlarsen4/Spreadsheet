```
Author:				Hannah Larsen
Partner:			Todd Oldham
StartDate:			13-Jan-2023
Course:				CS3500, University of Utah, School of Computing
GitHubID:			25hlarsen4, Destroyr-of-u
Solution:			Spreadsheet
Copyright:			CS3500, Hannah Larsen, Todd Oldham - This work may not be copied for use in academic coursework.
```


# Overview of the Spreadsheet Functionality:

- Spreadsheet cells are capable of holding whole numbers, decimals, or string values.
- Spreadsheet cells are capable of performing mathematical formula evaluations. The allowed operators are +, -, *, /, (, and ).
  The allowed operands are typed out numbers or references to other cells which must have numberic values. Spaces in formulas are ignored.
  Formula examples: =1+1, =-1+1, =(3-1)-(2+4), =2-(-2)
                    Then say cell A1 contains value -1, a forumla referencing A1 like so is valid: =A1+1
                    Note that implicit multiplication is not allowed. For example, you must do =2*(4-1), not =2(4-1)

- When you click on a cell/are focues on it, it will show the cell contents (either a typed out number or string value, or a formula that was entered)
- When you are no longer focused on a cell, it will show the actual cell value (either a typed out number or string value, or the evaluation of a formula in the cell)

# How to use:
- To fill a cell with an numerical or string value, click the cell, type the value, and hit enter.
- To fill a cell with a formula to be evaluated, for example to make cell C1 = A1 + B1, click cell C1 and type the desired formula, starting with an = sign, no spacing requirements.
  So for this example, click cell C1, type =A1+B1, and hit enter
- To save, provide an absolute path, including what you want the file to be called without an extension (ie C:\Users\name\Downloads\Spreadsheets\nameOfSpreadsheetFile). 
- To open a previously saved spreadsheet, provide the same path that you provided to save the spreadsheet.
