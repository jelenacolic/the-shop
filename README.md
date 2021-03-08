# the-shop

## Description

The shop is an application for ordering articles from suppliers, selling them to buyers, and displaying article information.<br/>
It's written in **.net core 3.1**. It uses in-memory database to store all needed information, but that can be easily changed to suit client needs because of the Dependency Injection.

Business logic is covered with unit tests using **xUnit**. 

Info logs can be found in **.infolog** file inside **Logs** directory.<br/>
Error logs can be found in **.log** file inside **Logs** directory.

## Build and run

To run application, you need to download the code, build it and then run **TheShop.exe.**

When run, the app adds example suppliers, articles and buyers, so it has something to work with.<br/> 
After that the shop tries to order an article and sell it to buyer. <br/> 
Lastly, the shop tries to get 2 different articles and prints out if they are found or not.
