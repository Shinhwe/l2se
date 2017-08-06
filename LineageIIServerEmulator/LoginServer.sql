CREATE DATABASE l2se;
USE l2se;
CREATE TABLE account
(
	id int NOT NULL PRIMARY KEY identity(1,1),
	[login] varchar(45) NOT NULL,
	[password] varchar(255) NOT NULL,
	access tinyint NOT NULL,
	lastServer tinyint NOT NULL,
	[status] tinyint NOT NULL,
	loginAt datetime NOT NULL DEFAULT GETDATE(),
	updateAt datetime NOT NULL DEFAULT GETDATE(),
	createAt datetime NOT NULL DEFAULT GETDATE()
)
