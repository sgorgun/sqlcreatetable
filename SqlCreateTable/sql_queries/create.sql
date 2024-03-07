CREATE TABLE [dbo].[emploee] (
    [e_id]      INT           IDENTITY (1, 1) NOT NULL,
    [e_name]    NVARCHAR (50) NULL,
    [e_surname] NVARCHAR (50) NULL,
    CONSTRAINT [PK_emploee] PRIMARY KEY CLUSTERED ([e_id] ASC)
);

CREATE TABLE [dbo].[project] (
    [p_id]                INT           IDENTITY (1, 1) NOT NULL,
    [p_name]              NVARCHAR (50) NULL,
    [p_date_of _creation] DATE          NULL,
    [p_status]            CHAR (10)     NULL,
    [p_data_of_closure]   DATE          NULL,
    CONSTRAINT [PK_project] PRIMARY KEY CLUSTERED ([p_id] ASC)
);

CREATE TABLE [dbo].[position] (
    [pos_id]   INT        IDENTITY (1, 1) NOT NULL,
    [pos_name] NCHAR (10) NULL,
    CONSTRAINT [PK_position] PRIMARY KEY CLUSTERED ([pos_id] ASC)
);

CREATE TABLE [dbo].[m2m_project_emploee] (
    [e_id]   INT NOT NULL,
    [p_id]   INT NOT NULL,
    [pos_id] INT NOT NULL,
    CONSTRAINT [FK_m2m_project_emploee_emploee] FOREIGN KEY ([e_id]) REFERENCES [dbo].[emploee] ([e_id]),
    CONSTRAINT [FK_m2m_project_emploee_project] FOREIGN KEY ([p_id]) REFERENCES [dbo].[project] ([p_id]),
    CONSTRAINT [FK_m2m_project_emploee_position] FOREIGN KEY ([pos_id]) REFERENCES [dbo].[position] ([pos_id])
);

CREATE TABLE [dbo].[status] (
    [s_id]   INT           IDENTITY (1, 1) NOT NULL,
    [s_name] NVARCHAR (50) NULL,
    CONSTRAINT [PK_status] PRIMARY KEY CLUSTERED ([s_id] ASC)
);

CREATE TABLE [dbo].[task] (
    [t_id]          INT           IDENTITY (1, 1) NOT NULL,
    [p_id]          INT           NOT NULL,
    [t_description] NVARCHAR (50) NULL,
    [t_deadline]    DATE          NULL,
    CONSTRAINT [PK_task] PRIMARY KEY CLUSTERED ([t_id] ASC),
    CONSTRAINT [FK_task_project] FOREIGN KEY ([p_id]) REFERENCES [dbo].[project] ([p_id])
);

CREATE TABLE [dbo].[m2m_task_status] (
    [s_id]   INT  NOT NULL,
    [t_id]   INT  NOT NULL,
    [s_date] DATE NULL,
    [e_id]   INT  NULL,
    CONSTRAINT [FK_m2m_task_status_emploee] FOREIGN KEY ([e_id]) REFERENCES [dbo].[emploee] ([e_id]),
    CONSTRAINT [FK_m2m_task_status_task1] FOREIGN KEY ([t_id]) REFERENCES [dbo].[task] ([t_id]),
    CONSTRAINT [FK_m2m_task_status_status1] FOREIGN KEY ([s_id]) REFERENCES [dbo].[status] ([s_id])
);

