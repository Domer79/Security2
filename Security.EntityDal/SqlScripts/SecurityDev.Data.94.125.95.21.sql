USE [SecurityDev]
GO
SET IDENTITY_INSERT [sec].[Members] ON 

GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (7, N'Group1')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (4, N'Group3')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (5, N'Group4')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (1, N'User1')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (6, N'User2')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (2, N'Группа5')
GO
INSERT [sec].[Members] ([idMember], [name]) VALUES (3, N'Группа7')
GO
SET IDENTITY_INSERT [sec].[Members] OFF
GO
INSERT [sec].[Groups] ([idMember], [description]) VALUES (2, NULL)
GO
INSERT [sec].[Groups] ([idMember], [description]) VALUES (3, NULL)
GO
INSERT [sec].[Groups] ([idMember], [description]) VALUES (4, NULL)
GO
INSERT [sec].[Groups] ([idMember], [description]) VALUES (5, NULL)
GO
INSERT [sec].[Groups] ([idMember], [description]) VALUES (7, NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (1, 0xB081DBE85E1EC3FFC3D4E7D0227400CD, N'u1', N'u1', NULL, N'u1@m.r', 0, CAST(N'2016-12-02 12:39:11.9470000' AS DateTime2), NULL)
GO
INSERT [sec].[Users] ([idMember], [password], [firstName], [lastName], [middleName], [email], [status], [dateCreated], [lastActivityDate]) VALUES (6, 0xB081DBE85E1EC3FFC3D4E7D0227400CD, N'u2', N'u2', NULL, N'u2@m.r', 0, CAST(N'2016-12-02 12:46:34.1130000' AS DateTime2), NULL)
GO
INSERT [sec].[UserGroups] ([idUser], [idGroup]) VALUES (1, 2)
GO
INSERT [sec].[UserGroups] ([idUser], [idGroup]) VALUES (1, 3)
GO
SET IDENTITY_INSERT [sec].[Roles] ON 

GO
INSERT [sec].[Roles] ([idRole], [name], [description]) VALUES (1, N'Role1', NULL)
GO
SET IDENTITY_INSERT [sec].[Roles] OFF
GO
SET IDENTITY_INSERT [sec].[AccessTypes] ON 

GO
INSERT [sec].[AccessTypes] ([idAccessType], [name]) VALUES (1, N'Add')
GO
INSERT [sec].[AccessTypes] ([idAccessType], [name]) VALUES (3, N'Delete')
GO
INSERT [sec].[AccessTypes] ([idAccessType], [name]) VALUES (4, N'Select')
GO
INSERT [sec].[AccessTypes] ([idAccessType], [name]) VALUES (2, N'Update')
GO
SET IDENTITY_INSERT [sec].[AccessTypes] OFF
GO
SET IDENTITY_INSERT [sec].[SecObjects] ON 

GO
INSERT [sec].[SecObjects] ([idSecObject], [ObjectName], [Type], [Discriminator]) VALUES (1, N'SecObject1', NULL, NULL)
GO
SET IDENTITY_INSERT [sec].[SecObjects] OFF
GO
INSERT [sec].[Grants] ([idSecObject], [idRole], [idAccessType]) VALUES (1, 1, 4)
GO
INSERT [sec].[MemberRoles] ([idMember], [idRole]) VALUES (1, 1)
GO
