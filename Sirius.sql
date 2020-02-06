
CREATE TABLE [dbo].[status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar] (100) NOT NULL,
CONSTRAINT [PK_status] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[posts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar] (100) NOT NULL,
CONSTRAINT [PK_posts] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[deps](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar] (100) NOT NULL,
CONSTRAINT [PK_deps] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[persons](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [varchar] (100) NOT NULL,
	[second_name] [varchar] (100) NOT NULL,
	[last_name] [varchar] (100) NOT NULL,
	[date_employ] [datetime] NULL,
	[date_uneploy] [datetime] NULL,
	[status] [int] NOT NULL,
	[id_dep] [int] NOT NULL,
	[id_post] [int] NOT NULL,
CONSTRAINT [PK_ersonss] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

INSERT status
(name)
values
('stat1'),
('stat2'),
('stat3');

INSERT posts
(name)
values
('intern'),
('junior'),
('middle'),
('senior');

INSERT deps
(name)
values
('HR'),
('IT'),
('BUH');

INSERT persons
(first_name,second_name,last_name,date_employ,date_uneploy,status,id_post,id_dep)
values
('Ivan','Ivanovich','Ivanov',DATEADD(DAY,-40,GETDATE()),NULL,1,3,1),
('Dmitriy','Ivanovich','Ivanov',DATEADD(DAY,-80,GETDATE()),NULL,2,1,1),
('Kuzma','Ivanovich','Ivanov',DATEADD(DAY,-85,GETDATE()),DATEADD(DAY,-40,GETDATE()),1,1,2),
('Kirill','Ivanovich','Ivanov',DATEADD(DAY,-85,GETDATE()),NULL,3,3,1),
('Nikita','Ivanovich','Ivanov','20191220 12:20:36',NULL,2,4,3),
('Shura','Ivanovich','Ivanov',DATEADD(DAY,-85,GETDATE()),NULL,1,3,3),
('Alexander','Ivanovich','Ivanov','20191220 12:20:38',DATEADD(DAY,-38,GETDATE()),1,4,2),
('Oleg','Ivanovich','Ivanov',DATEADD(DAY,-85,GETDATE()),DATEADD(DAY,-100,GETDATE()),3,3,1);

DELETE FROM status
DROP TABLE [dbo].[status]

SELECT * FROM status
SELECT * FROM deps
SELECT * FROM posts
SELECT * FROM persons

SELECT first_name,second_name,last_name,date_employ,date_uneploy,posts.name AS post,deps.name AS dep, status.name AS status FROM persons
	JOIN deps
	ON persons.id_dep=deps.id
	JOIN posts
	ON persons.id_post=posts.id
	JOIN [status]
	ON persons.status=[status].id

SELECT CONCAT(last_name,' ', SUBSTRING(first_name,1,1),'. ',SUBSTRING(second_name,1,1),'.' ) as FIO,posts.name AS post,deps.name AS dep, status.name AS status,date_employ,date_uneploy FROM persons
	JOIN deps
	ON persons.id_dep=deps.id
	JOIN posts
	ON persons.id_post=posts.id
	JOIN [status]
	ON persons.status=[status].id


DROP PROCEDURE person_with_stat_with_emp
DROP PROCEDURE person_with_stat_with_unep 

CREATE PROCEDURE person_with_stat_with_emp 
@date_start [date],
@date_end [date],
@stat [varchar] (100)
AS
BEGIN
	SELECT cast(date_employ as date) as [date],COUNT(*) as count FROM persons 
	JOIN [status]
	ON persons.status=[status].id
	where [status].[name] = @stat And date_employ>@date_start AND date_employ<@date_end
	group by cast(date_employ as date)
END;

CREATE PROCEDURE person_with_stat_with_unep 
@date_start [date],
@date_end [date],
@stat [varchar] (100)
AS
BEGIN
	SELECT cast(date_uneploy as date) as [date],COUNT(*) as count FROM persons 
	JOIN [status]
	ON persons.status=[status].id
	where [status].[name] = @stat And date_uneploy>@date_start AND date_uneploy<@date_end
	group by cast(date_uneploy as date)
END;

CREATE PROCEDURE stat_list AS
BEGIN
	SELECT [name] from [status]
END;

CREATE PROCEDURE Persons_list AS
BEGIN
	SELECT CONCAT(last_name,' ', SUBSTRING(first_name,1,1),'. ',SUBSTRING(second_name,1,1),'.' ) as FIO,posts.name AS post,deps.name AS dep, status.name AS status,date_employ,date_uneploy FROM persons
	JOIN deps
	ON persons.id_dep=deps.id
	JOIN posts
	ON persons.id_post=posts.id
	JOIN [status]
	ON persons.status=[status].id
END;

exec person_with_stat_with_emp '20190120','20200206','stat3'

exec Persons_list

exec person_with_stat_with_unep '20190120','20200206','stat3'

exec stat_list

