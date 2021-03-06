USE [StoreSecurity]
GO
SET IDENTITY_INSERT [sec].[Members] ON 

GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (10, N'akoshelenko')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (9, N'avdeev')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (11, N'Domer')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (5, N'dshamonin')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (2, N'Hima')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (4, N'lesha')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (3, N'PSS')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (1, N'sa')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (7, N'sparrow')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (6, N'su873')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (8, N'trofimov')
GO
SET IDENTITY_INSERT [sec].[Members] OFF
GO
INSERT [sec].[Groups] ([idMember], [description]) VALUES (3, NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (1, 0x587744FE5D415B04506CE668DB9EFD5B, N'sa', N'sa', NULL, N'sa@domain.ru', 1, CAST(N'2016-12-02 10:04:31.8900000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (2, 0x674945F754805E4790563CFC9E6430D3, N'Азат', N'Нураев', NULL, N'ANuraev@itpss.ru', 1, CAST(N'2016-12-02 10:04:31.8900000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (4, 0x587744FE5D415B04506CE668DB9EFD5B, N'lesha', N'lesha', NULL, N'lesha@domain.ru', 1, CAST(N'2016-12-02 10:04:31.8900000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (5, 0xCE0BFD15059B68D67688884D7A3D3E8C, N'dshamonin', N'dshamonin', NULL, N'dshamonin@domain.ru', 1, CAST(N'2016-12-02 10:04:31.8900000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (6, 0x587744FE5D415B04506CE668DB9EFD5B, N'su873', N'su873', NULL, N'su873@domain.ru', 1, CAST(N'2016-12-02 10:04:31.8900000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (7, 0xB4E777EA2F751AA1A0CC4BF7A4ABEE28, N'sparrow', N'sparrow', NULL, N'sparrow@domain.ru', 1, CAST(N'2016-12-02 10:04:31.8900000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (8, 0xCE0BFD15059B68D67688884D7A3D3E8C, N'trofimov', N'trofimov', NULL, N'trofimov@domain.ru', 1, CAST(N'2016-12-02 10:04:31.8900000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (9, 0xCE0BFD15059B68D67688884D7A3D3E8C, N'avdeev', N'avdeev', NULL, N'avdeev@domain.ru', 1, CAST(N'2016-12-02 10:04:31.8900000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (10, 0x8B29265D7A5BE24E264798140983D92D, N'akoshelenko', N'akoshelenko', NULL, N'akoshelenko@domain.ru', 1, CAST(N'2016-12-02 10:04:31.8900000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (11, 0x6ECE4FD51BC113942692637D9D4B860E, N'Дамир', N'Гарипов', NULL, N'dgaripov@it.ru', 1, CAST(N'2016-12-02 10:16:51.8570000' AS DateTime2), NULL)
GO
INSERT [sec].[UserGroups] ([idUser], [idGroup]) VALUES (2, 3)
GO
INSERT [sec].[UserGroups] ([idUser], [idGroup]) VALUES (5, 3)
GO
INSERT [sec].[UserGroups] ([idUser], [idGroup]) VALUES (7, 3)
GO
INSERT [sec].[UserGroups] ([idUser], [idGroup]) VALUES (8, 3)
GO
INSERT [sec].[UserGroups] ([idUser], [idGroup]) VALUES (9, 3)
GO
INSERT [sec].[UserGroups] ([idUser], [idGroup]) VALUES (10, 3)
GO
INSERT [sec].[UserGroups] ([idUser], [idGroup]) VALUES (11, 3)
GO
SET IDENTITY_INSERT [sec].[Roles] ON 

GO
INSERT [sec].[Roles] ([idRole], [name], [description]) VALUES (1, N'Администратор', NULL)
GO
SET IDENTITY_INSERT [sec].[Roles] OFF
GO
SET IDENTITY_INSERT [sec].[AccessTypes] ON 

GO
INSERT [sec].[AccessTypes] ([idAccessType], [name]) VALUES (1, N'Add')
GO
INSERT [sec].[AccessTypes] ([idAccessType], [name]) VALUES (3, N'Delete')
GO
INSERT [sec].[AccessTypes] ([idAccessType], [name]) VALUES (5, N'Exec')
GO
INSERT [sec].[AccessTypes] ([idAccessType], [name]) VALUES (4, N'Select')
GO
INSERT [sec].[AccessTypes] ([idAccessType], [name]) VALUES (2, N'Update')
GO
SET IDENTITY_INSERT [sec].[AccessTypes] OFF
GO
SET IDENTITY_INSERT [sec].[SecObjects] ON 

GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (1, N'Main/', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (2, N'Main/GetStructuredCatalogs', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (3, N'Main/AddNewProduct', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (4, N'Main/GetProducts', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (5, N'Main/_LoginPartial', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (6, N'Main/AddNewCategory', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (7, N'Main/AddNewSubcategory', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (8, N'Main/AddNewCatalog', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (9, N'Main/DeleteCategory', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (10, N'Main/DeleteCatalog', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (11, N'Main/DeleteSubcategory', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (12, N'Main/GetCatalog', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (13, N'Main/UpdateRegularAttributes', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (14, N'Main/AddNewProperty', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (15, N'Main/AddNewPropertyGroup', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (16, N'Main/DeleteProperty', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (17, N'Main/UpdateProperty', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (18, N'Main/DeletePropertyGroup', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (19, N'Main/UpdatePropertyGroup', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (20, N'Main/DeleteProduct', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (21, N'Main/GetProduct', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (22, N'Main/UpdateProduct', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (23, N'Home/', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (24, N'Home/Create', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (25, N'Test/', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (26, N'Test/Test1', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (27, N'Test/Test2', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (28, N'Test/Test3', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (29, N'Test/Test4', NULL, NULL)
GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (30, N'Test/Test5', NULL, NULL)
GO
SET IDENTITY_INSERT [sec].[SecObjects] OFF
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (1, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (2, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (3, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (4, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (5, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (6, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (7, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (8, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (9, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (10, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (11, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (12, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (13, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (14, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (15, 1, 3)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (15, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (16, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (17, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (18, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (19, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (20, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (21, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (22, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (23, 1, 1)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (23, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (24, 1, 3)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (24, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (25, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (26, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (27, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (28, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (29, 1, 5)
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (30, 1, 5)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (1, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (2, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (3, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (4, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (5, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (6, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (7, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (8, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (9, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (10, 1)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (11, 1)
GO
