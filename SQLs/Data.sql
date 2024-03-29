/*Rows for sports*/
INSERT [dbo].[Sports] ([Name]) VALUES ('Futbol');
INSERT [dbo].[Sports] ([Name]) VALUES ('Tenis');
INSERT [dbo].[Sports] ([Name]) VALUES ('Basket');
INSERT [dbo].[Sports] ([Name]) VALUES ('Balonmano');
INSERT [dbo].[Sports] ([Name]) VALUES ('Padel');

/*Rows for courts*/
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('A1', 1);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('98A', 2);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('A3', 1);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('A4', 1);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('A5', 2);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('B1', 2);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('B2', 2);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('B3', 3);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('B4', 3);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('C1', 4);
INSERT [dbo].[Courts] ([Reference], [SportId]) VALUES ('C2', 4);

/*Rows for Members*/
INSERT [dbo].[Members] ([Name], [Surname], [Phone], [Address]) VALUES ('Juan Antonio', 'Domenech Rubio', '646664144', 'Carrer Elx 9, Mutxamel');
INSERT [dbo].[Members] ([Name], [Surname], [Phone], [Address]) VALUES ('Enric', 'Juliana Sanchez', '+34 650614999', 'Calle Buenaventura 87 3A, El Campello');
INSERT [dbo].[Members] ([Name], [Surname], [Phone], [Address]) VALUES ('Carlos', 'Villaescusa De Miguel', '856784143', 'Gran Via, Alicante');

/*Rows for Bookings*/
INSERT [dbo].[Bookings] ([CourtId], [MemberId], [Reservation]) VALUES (2, 2, CAST('2019-12-10T12:00:00.000' AS DateTime));
INSERT [dbo].[Bookings] ([CourtId], [MemberId], [Reservation]) VALUES (1, 2, CAST('2019-12-15T20:00:00.000' AS DateTime));
INSERT [dbo].[Bookings] ([CourtId], [MemberId], [Reservation]) VALUES (1, 3, CAST('2019-12-10T09:00:00.000' AS DateTime));
INSERT [dbo].[Bookings] ([CourtId], [MemberId], [Reservation]) VALUES (1, 3, CAST('2019-12-19T09:00:00.000' AS DateTime));

/*Rows for Users*/
INSERT [dbo].[Users] ([FirstName], [LastName], [Email], [PasswordHash], [PasswordSalt]) VALUES ('Juan Antonio', 'Domenech Rubio', 'domejuanan@gmail.com', 0xF9DE27F120FC07ADFCFBA6839E23B915C23347013AC00A192D986A11D5157C6324E31B0E7AD4AEEEBE7A1999D42C4DFA9A2B6130FBDC24E72DE110395014CA69, 0x0351A5F800FF82CBDC797CD35DA216708568F8E1E8C27F2C2DACFB17E2EDE299BFE9ABFA2CB90A6491F199C7DE14113BCBE885C088A1922DE2E18527979081D47C632B545DE778F9C560EB764F499CC1AD983470BEDEE04BBA9D2B3EB1F30A1D68D27120298A9609C7E8C652C4024B5AB77FF78F3C2BD648A58BBC973D04E5A3);