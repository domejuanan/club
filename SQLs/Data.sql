SET IDENTITY_INSERT [dbo].[Sports] ON 

INSERT [dbo].[Sports] ([Id], [Name]) VALUES (1, N'Futbol')
INSERT [dbo].[Sports] ([Id], [Name]) VALUES (2, N'Tenis')
INSERT [dbo].[Sports] ([Id], [Name]) VALUES (3, N'Basket')
INSERT [dbo].[Sports] ([Id], [Name]) VALUES (4, N'Balonmano')
INSERT [dbo].[Sports] ([Id], [Name]) VALUES (8, N'Padel')
SET IDENTITY_INSERT [dbo].[Sports] OFF
SET IDENTITY_INSERT [dbo].[Courts] ON 

INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (1, N'A1', 1)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (2, N'98A', 2)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (3, N'A3', 1)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (4, N'A4', 1)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (5, N'A5', 2)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (6, N'B1', 2)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (7, N'B2', 2)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (8, N'B3', 3)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (9, N'B4', 3)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (10, N'C1', 4)
INSERT [dbo].[Courts] ([Id], [Reference], [SportId]) VALUES (11, N'C2', 4)
SET IDENTITY_INSERT [dbo].[Courts] OFF
SET IDENTITY_INSERT [dbo].[Members] ON 

INSERT [dbo].[Members] ([Id], [Name], [Surname], [Phone], [Address]) VALUES (1, N'Juan Antonio', N'Domenech Rubio', N'646664144', N'Carrer Elx 9, Mutxamel')
INSERT [dbo].[Members] ([Id], [Name], [Surname], [Phone], [Address]) VALUES (2, N'Enric', N'Juliana Sanchez', N'+34 650614999', N'Calle Buenaventura 87 3A, El Campello')
INSERT [dbo].[Members] ([Id], [Name], [Surname], [Phone], [Address]) VALUES (4, N'Carlos', N'Villaescusa De Miguel', N'856784143', N'Gran Via, Alicante')
SET IDENTITY_INSERT [dbo].[Members] OFF
SET IDENTITY_INSERT [dbo].[Bookings] ON 

INSERT [dbo].[Bookings] ([Id], [CourtId], [MemberId], [Reservation]) VALUES (4, 2, 2, CAST(N'2019-12-10T12:00:00.000' AS DateTime))
INSERT [dbo].[Bookings] ([Id], [CourtId], [MemberId], [Reservation]) VALUES (5, 1, 2, CAST(N'2019-12-15T20:00:00.000' AS DateTime))
INSERT [dbo].[Bookings] ([Id], [CourtId], [MemberId], [Reservation]) VALUES (6, 1, 4, CAST(N'2019-12-10T09:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Bookings] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [PasswordHash], [PasswordSalt]) VALUES (5, N'Lara', N'Monclús Muñoz', N'laramonclus@hotmail.', 0x113C2B1B7D9160B4A4202C94D3E16997516B2C7361CF405C6D72C8DC8839F0FBA2EC9C7EA8C90258935ECDDFC9ADDB5ED01F12D43EB7CC023E67B1A09064FF3E, 0xBC8B5534E546D4D2BB397A9B49BB390200BD77F0B9D7559C84BEBE5D22C64332EACC12DD3FBF58F69A64F7A3F8C5ACFE73EE3DDDEF48EE47D2D3BF3F798451F695DD9390CE73DEE4420F91961CD7F2A303189B1B6578325C8F0A243BD68E7FD32CE66584D74CD1704E7FCBB45C3FF8918DCD6A89F833494C532C031E79ACB037)
INSERT [dbo].[Users] ([Id], [FirstName], [LastName], [Email], [PasswordHash], [PasswordSalt]) VALUES (6, N'Juan Antonio', N'Domenech Rubio', N'domejuanan@gmail.com', 0xF9DE27F120FC07ADFCFBA6839E23B915C23347013AC00A192D986A11D5157C6324E31B0E7AD4AEEEBE7A1999D42C4DFA9A2B6130FBDC24E72DE110395014CA69, 0x0351A5F800FF82CBDC797CD35DA216708568F8E1E8C27F2C2DACFB17E2EDE299BFE9ABFA2CB90A6491F199C7DE14113BCBE885C088A1922DE2E18527979081D47C632B545DE778F9C560EB764F499CC1AD983470BEDEE04BBA9D2B3EB1F30A1D68D27120298A9609C7E8C652C4024B5AB77FF78F3C2BD648A58BBC973D04E5A3)
SET IDENTITY_INSERT [dbo].[Users] OFF
