ALTER TABLE [All_Leaves]
ADD CONSTRAINT fk_AllLeaves_UserID_Users_UserID FOREIGN KEY (UserID)
REFERENCES Users(UserID);

select*from[Users]

select*from[All_Leaves]

insert into All_Leaves () values ();

UPDATE All_Leaves SET IsApprove = null WHERE LeaveID = 21 ;
UPDATE Users SET IsActive = 1 WHERE UserID = 4 ;

select * from [All_Leaves] order by 1  desc

UPDATE Users SET Email = 'test1@gmail.com' WHERE UserID = 3 ;
UPDATE Users SET Email = 'test2@gmail.com' WHERE UserID = 4 ;