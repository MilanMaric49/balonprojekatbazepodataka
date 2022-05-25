create database Sportski_objekat
use Sportski_objekat

create table Korisnik(
id int primary key identity(1,1),
email varchar(50) not null,
lozinka varchar(30) not null,
ime varchar(20) not null,
prezime varchar(50) not null
);
create table Objekat(
id int primary key identity(1,1),
naziv varchar(100) not null,
adresa varchar(100) not null,
opis varchar(1000)
);
create table Zaposleni(
id int primary key identity(1,1),
email varchar(50) not null,
lozinka varchar(30) not null,
ime varchar(20) not null,
prezime varchar(50) not null,
objekat_id int foreign key references Objekat(id)
);
create table Termini(
id int primary key identity(1,1),
datum date not null,
vreme int not null,
cena int not null,
objekat_id int foreign key references Objekat(id)
);
create table Rezervacija(
id int primary key identity(1,1),
korisnik_id int foreign key references Korisnik(id),
objekat_id int foreign key references Objekat(id),
datum date not null,
pocetak int not null,
kraj int not null
);

insert into Korisnik values('milanmaric@gmail.com', 'milan123', 'Milan', 'Maric')
insert into Korisnik values('dusanstankovic@gmail.com', 'duleCR7', 'Dusan', 'Stankovic')
insert into Korisnik values('bogdankolovic@gmail.com', 'autizmo', 'Bogdan', 'Kolovic')
insert into Korisnik values('andrejbratic@gmail.com', 'bratke123', 'Andrej', 'Bratic')
select * from korisnik

insert into Objekat values('Balon Juzni Bulevar', 'Juzni bulevar 69', 'Parking, tusevi, teren za 5 na 5 ili 6 na 6')
insert into Objekat values('Zlatna lopta Vracar', 'Djerdapska 19', 'Parking, tusevi, teren za 5 na 5 ili 6 na 6')
insert into Objekat values('Imperija sport', 'Vukosaviceva 21a', 'Parking, tusevi, teren za 5 na 5')
select * from Objekat

insert into zaposleni values('milanmariczp@gmail.com', 'milan123', 'Milan', 'Maric', 1)
select * from zaposleni

insert into termini values('2022-05-22', 8, 3500, 1);
insert into termini values('2022-05-22', 9, 3500, 1);
insert into termini values('2022-05-22', 10, 3500, 1);
insert into termini values('2022-05-22', 11, 3500, 1);
insert into termini values('2022-05-22', 12, 3500, 1);
insert into termini values('2022-05-22', 13, 4000, 1);
insert into termini values('2022-05-22', 14, 4000, 1);
insert into termini values('2022-05-22', 15, 4000, 1);

insert into termini values('2022-05-23', 8, 3500, 1);
insert into termini values('2022-05-23', 9, 3500, 1);
insert into termini values('2022-05-23', 10, 3500, 1);
insert into termini values('2022-05-23', 11, 3500, 1);
insert into termini values('2022-05-23', 12, 3500, 1);
insert into termini values('2022-05-23', 13, 4000, 1);
insert into termini values('2022-05-23', 14, 4000, 1);
insert into termini values('2022-05-23', 15, 4000, 1);

insert into termini values('2022-05-26', 8, 3500, 1);
insert into termini values('2022-05-26', 9, 3500, 1);
insert into termini values('2022-05-26', 10, 3500, 1);
insert into termini values('2022-05-26', 11, 3500, 1);
insert into termini values('2022-05-26', 12, 3500, 1);
insert into termini values('2022-05-26', 13, 4000, 1);
insert into termini values('2022-05-26', 14, 4000, 1);
insert into termini values('2022-05-26', 15, 4000, 1);

insert into termini values('2022-05-27', 8, 3500, 1);
insert into termini values('2022-05-27', 9, 3500, 1);
insert into termini values('2022-05-27', 10, 3500, 1);
insert into termini values('2022-05-27', 11, 3500, 1);
insert into termini values('2022-05-27', 12, 3500, 1);
insert into termini values('2022-05-27', 13, 4000, 1);
insert into termini values('2022-05-27', 14, 4000, 1);
insert into termini values('2022-05-27', 15, 4000, 1);

select * from termini
select * from rezervacija;
delete from rezervacija
select * from zaposleni where id = 1

create procedure rezervacija_dodaj
@korisnik_id int,
@objekat_id int,
@datum date,
@pocetak int,
@kraj int
as
begin try
if (exists(select top 1 id from Rezervacija where datum = @datum and ((@pocetak >= pocetak and @pocetak <= kraj) or (@kraj >= pocetak and @kraj <= kraj)))) return -1;
insert into Rezervacija values(@korisnik_id, @objekat_id, @datum, @pocetak, @kraj);
return 0;
end try
begin catch
return @@error;
end catch;

create procedure termin_dodaj
@datum date,
@vreme int,
@cena int,
@objekat_id int
as
begin try
if (exists(select top 1 id from Termini where datum = @datum and vreme = @vreme and objekat_id = @objekat_id)) return -1;
insert into Termini values(@datum, @vreme, @cena, @objekat_id);
return 0;
end try
begin catch
return @@error;
end catch;