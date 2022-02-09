# Database design and SQL (DDL)

## Task  

Create a database scheme for the domain (see description). The BD scheme must correspond to at least the third normal form (3NF).

DB should contain at least 10 tables with primary and foreign keys, unique, not null attributes where it is suitable. Do not use ALTER operations and cascading deletion from tables.

### Domain description   

- The company employees carry out projects (project data: name, date of creation, status open / closed, date of closure).   
- An employee can carry out several projects, on different projects his position may differ.  
- Tasks for the project are issued to the employee with a deadline. The task can be in the status: open, completed, requires revision, accepted (closed), indicating the date of setting the status and the employee setting the status. 

### How to complete task solution

1. Design DB scheme and save it’s screenshot in file **DBScheme.jpg**. Upload it to folder **sql_queries**. 
1. Save script with queries to file **sql_queries\create.sql**. Separate queries with “;”.
______
