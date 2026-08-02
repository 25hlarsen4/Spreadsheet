```
Author:				Hannah Larsen
Partner:			Todd Oldham
Course:				CS3500, University of Utah, School of Computing
Copyright:			CS3500, Hannah Larsen, Todd Oldham - This work may not be copied for use in academic coursework.
```


# Overview of the Spreadsheet Functionality:

A spreadsheet application developed in C# using .NET MAUI for the University of Utah CS 3500 course. The application supports formula evaluation, dependency tracking, file save/load, and a spreadsheet interface.

**Check out this quick [demo video](https://youtu.be/GNep11FJyEo?si=FebOURxE61NcoyDV) of the project.**

- Spreadsheet cells are capable of holding whole numbers, decimals, or string values.
- Spreadsheet cells are capable of performing mathematical formula evaluations. The allowed operators are +, -, \*, /, (, and ).
  The allowed operands are typed out numbers or references to other cells which must have numeric values. Spaces in formulas are ignored.
  Formula examples: =1+1, =-1+1, =(3-1)-(2+4), =2-(-2)
                    Then say cell A1 contains a numeric value, a formula referencing A1 like so is valid: =A1+1
                    Note that implicit multiplication is not allowed. For example, you must do =2\*(4-1), not =2(4-1)

- When you click on a cell/are focused on it, it will show the cell contents (either a typed out number or string value, or a formula that was entered)
- When you are no longer focused on a cell, it will show the actual cell value (either a typed out number or string value, or the evaluation of a formula in the cell)
- You can save/load files through the file menu.

## Project Structure

- **Spreadsheet** – Spreadsheet engine
- **GUI** – .NET MAUI graphical interface
- **FormulaEvaluator** – Expression parsing and evaluation
- **DependencyGraph** - Cell dependency tracking

## Requirements

- Visual Studio
- .NET 10 SDK
- .NET MAUI workload

## Running the Project

1. Clone the repository.
2. Open the solution (`Spreadsheet.sln`) in Visual Studio.
3. Restore NuGet packages if prompted.
4. Set the **GUI** project as the startup project.
5. Select **Windows Machine** as the run target.
6. Build and run the project.

# How to use:
- To fill a cell with a numerical or string value, click the cell, type the value, and hit enter.
- To fill a cell with a formula to be evaluated, for example to make cell C1 = A1 + B1, click cell C1 and type the desired formula, starting with an = sign, no spacing requirements.
  So for this example, click cell C1, type =A1+B1, and hit enter.
- To save, provide an absolute path, including what you want the file to be called without an extension (ie C:\Users\name\Downloads\Spreadsheets\nameOfSpreadsheetFile). 
- To open a previously saved spreadsheet, provide the same path that you provided to save the spreadsheet.
