```
Author:			Hannah Larsen
Partner:		Todd Oldham
Course:			CS3500, University of Utah, School of Computing
GitHubID:		25hlarsen4, Destroyr-of-u
Repo:			https://github.com/uofu-cs3500-spring23/assignment-six---gui---functioning-spreadsheet-larsenoldham.git
CommitDate:		02-March-2023, 2:40 pm
Project:	  	GUI
Copyright:		CS3500, Hannah Larsen, and Todd Oldham - This work may not be copied for use in academic coursework.
```


# Partnership:
All code was completed via pair programming, however we each spent time separately researching different topics that
we had run into issues with.


# Branching:
An A6Development branch was created for us both to work on. Upon the completion of the assignment, we merged it into master.


# Additional Features and Design Decisions:
Our additional feature was a clear entry button. If the user wants to delete a cell with particularly long contents, instead 
of backspacing everything and hitting enter, they can simply select the desired cell by clicking on it and then hit the 
'clear entry' button provided. Furthermore, dependent cells will not be cleared in case they want the dependencies to remain.
In terms of our design decisions, we decided it would be easiest to use the starter code we were given, but we added extra 
labels/ entries to the top bar to make the GUI look more visually pleasing.


# Comments to Evaluators:
Maui had issues with us setting a default focused cell, but a TA said that it was not a requirement and to not worry about it 
as MAUI had issues with it last year as well.
Furthermore, our GUI requires that to save your file, you must provide a FULL file path, since MAUI had access issues when 
trying to call Spreadsheet.Save(file_name_without_full_path), even though that works perfectly fine in our Spreadsheet testing.
We consulted a TA regarding this, and he said requiring a full file path would be just fine and we wouldn't lose points for it.


# Assignment Specific Topics:
No additional write up was required by this assignment.


# Consulted Peers:
We only consulted with each other as partners and TAs on Piazza.


# References:
1. Canvas assignment
2. Piazza posts
3. Entry - https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/entry?view=net-maui-7.0
4. Display pop-ups - https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pop-ups?view=net-maui-7.0
5. StackLayout - https://learn.microsoft.com/en-us/dotnet/maui/user-interface/layouts/stacklayout?view=net-maui-7.0

