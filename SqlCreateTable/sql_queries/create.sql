CREATE TABLE Employee (
    id INTEGER PRIMARY KEY,
    e_name TEXT NOT NULL,
    e_surname TEXT NOT NULL
);

CREATE TABLE Project (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    creation_date DATE NOT NULL,
    p_status TEXT NOT NULL,
    closure_date DATE
);

CREATE TABLE Position (
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE EmployeeProject (
    id INTEGER UNIQUE PRIMARY KEY,
    employee_id INTEGER,
    project_id INTEGER,
    position_id INTEGER,
    FOREIGN KEY (employee_id) REFERENCES Employee(id),
    FOREIGN KEY (project_id) REFERENCES Project(id),
    FOREIGN KEY (position_id) REFERENCES Position(id)
);

CREATE TABLE Task (
    id INTEGER PRIMARY KEY,
    project_id INTEGER,
    description TEXT NOT NULL,
    deadline DATE NOT NULL,
    FOREIGN KEY (project_id) REFERENCES Project(id)
);

CREATE TABLE TaskStatus (
    id INTEGER PRIMARY KEY,
    t_status TEXT NOT NULL,
    set_date DATE NOT NULL
);

CREATE TABLE EmployeeTask (
    id INTEGER UNIQUE PRIMARY KEY,
    employee_id INTEGER,
    task_id INTEGER,
    status_id INTEGER,
    FOREIGN KEY (employee_id) REFERENCES Employee(id),
    FOREIGN KEY (task_id) REFERENCES Task(id),
    FOREIGN KEY (status_id) REFERENCES TaskStatus(id)
);