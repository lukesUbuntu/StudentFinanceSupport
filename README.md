# Student Finance Support
This was an Assignment i did for paper IT7x11 which was developed in ASP MVC within 2 weeks.

#Assignment Brief
The objective of this project was to develop an information system to streamline work for Student Financial Advisor for a university. The Advisor provides free and confidential advice on financial matters for students. An information system is developed to provide the client a graphical user interface at the front-end and a database at the back-end. This enables the client to insert, delete, update and retrieve student information/data

#Project Scope
-	The information system provides the client with a Graphical User Interface to allow the user to enter the information of new students into the database. 
-	The system allows the user to retrieve the information of an existing student and also update their information. 
-	The information system developed must be able to incorporate the change in client requirements during the development phase. 
-	The client would initially be provided with a working model with minimum functionality. The functionality is then added incrementally to complete this project.
-	The user may be able to view the breakdown of the budget spending (by Faculty) on food vouchers, petrol vouchers, and hardship grant on a yearly, monthly, weekly and up-to-date basis.

#Screen Shots
###Reports System
<img src=http://i.imgur.com/aKdxCNo.png>
###Forgot Password
<img src=http://i.imgur.com/bxDT7vE.png>
#Requirements Completed
List of requirements from the Student Financial report that was given and implemented into MVC 
-	Adding/Deleting student grants.
-	Adding/Editing/Deleting students.
-	Viewing student grants.
-	Adding/Editing/Deleting course names that were available.
-	Adding/Editing/Deleting grant types that were available.
-	Adding new administrators.
-	Searching for students.
-	Viewing the list of all students within the database.
-	Viewing individual profiles of each student which shows their full details.
-	User login/logout feature.
-	Password retrieval feature through email and text messaging
-	Generating reports based on different criteria such as month, year, campus, faculty, gender, and ethnicity 

# Added Features / Improvements Implemented

-	Implemented Bootstrap(sbadmin theme)
-	Implemented a menu search for options on main dashboard 
-	LINQ to query reports from a large amount of filters
-	From login page, if you load a custom link & you’re not logged in it will sign you in and redirect back to the page you where requesting.
-	Implemented a reports filter section 
-	Created Custom jQuery plugins I have written from previous projects 
-	Customize report options added stats & graphs with easy filtering options
-	Added custom roles manager with the following roles
    -	Admin (complete system management)
    -	Advisor (can add vouchers and reports)
    -	Accountant (list reports only)
-	Encrypted Passwords with bCrypt 
-	Visual graphical stats for with also full reporting
- Added in forgot password via TXT & email, with settings from webconfig
- Added a baseController



- Full Admin 
    - username : admin@example.com
    - password : test

- Advisor & Reports only
    - username : advisor@example.com
    - password : test

- Reports only
    - username : reports@example.com
    - password : test


