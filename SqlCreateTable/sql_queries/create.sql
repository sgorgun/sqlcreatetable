CREATE TABLE employee (
    e_id INT IDENTITY (1, 1) NOT NULL,
    e_name NVARCHAR (50) NULL,
    e_surname NVARCHAR (50) NULL,
    CONSTRAINT PK_employee PRIMARY KEY (e_id)
);

CREATE TABLE project (
    p_id INT IDENTITY (1, 1) NOT NULL,
    p_name NVARCHAR (50) NULL,
    p_date_of_creation DATE NULL,
    p_statuses CHAR (10) NULL,
    p_date_of_closure DATE NULL,
    CONSTRAINT PK_project PRIMARY KEY (p_id)
);

CREATE TABLE task (
    t_id INT IDENTITY (1, 1) NOT NULL,
    p_id INT NOT NULL,
    t_description NVARCHAR (50) NULL,
    t_deadline DATE NULL,
    CONSTRAINT PK_task PRIMARY KEY (t_id),
    CONSTRAINT FK_task_project_project FOREIGN KEY (p_id) REFERENCES project (p_id)
);

CREATE TABLE position (
    pos_id INT IDENTITY (1, 1) NOT NULL,
    pos_name NCHAR (10) NULL,
    CONSTRAINT PK_position PRIMARY KEY (pos_id)
);

CREATE TABLE statuses (
    s_id INT IDENTITY (1, 1) NOT NULL,
    s_name NVARCHAR (50) NULL,
    CONSTRAINT PK_statuses PRIMARY KEY (s_id)
);

CREATE TABLE m2m_project_employee (
    e_id INT NOT NULL,
    p_id INT NOT NULL,
    pos_id INT NOT NULL,
    CONSTRAINT PK_m2m_project_employee UNIQUE (e_id, p_id),
    CONSTRAINT FK_m2m_project_employee_employee FOREIGN KEY (e_id) REFERENCES employee (e_id),
    CONSTRAINT FK_m2m_project_employee_project FOREIGN KEY (p_id) REFERENCES project (p_id),
    CONSTRAINT FK_m2m_project_employee_position FOREIGN KEY (pos_id) REFERENCES position (pos_id)
);

CREATE TABLE m2m_task_statuses
(
    s_id INT NOT NULL,
    t_id INT NOT NULL,
    s_date DATE NULL,
    e_id INT NULL,
    CONSTRAINT PK_m2m_task_statuses UNIQUE (s_id, t_id),
    CONSTRAINT FK_m2m_task_statuses_employee FOREIGN KEY (e_id) REFERENCES employee (e_id),
    CONSTRAINT FK_m2m_task_statuses_task FOREIGN KEY (t_id) REFERENCES task (t_id),
    CONSTRAINT FK_m2m_task_statuses_statuses FOREIGN KEY (s_id) REFERENCES statuses (s_id)
);