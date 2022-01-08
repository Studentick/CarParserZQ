﻿create table models
(
	model_code nvarchar(35) primary key,
	f_period nvarchar(25), -- тут можно разбит на 2 даты, 
	-- 1(not null) - начало производства, 2 - завершение
	model_name nvarchar(50) not null,
	complectation_cipher nvarchar(50)
);

create table complectations
(
	complectation_code nvarchar(50) primary key,
	model_code nvarchar(35),
	f_period nvarchar(25),
	params nvarchar(max) not null,
	foreign key (model_code) references models(model_code)
);

create table subgroups
(
	id int primary key identity,
	name nvarchar(250) not null,
	group_name nvarchar(250) not null,
	UNIQUE (name, group_name)
);

create table details
(
	detail_code nvarchar(35) primary key,
	count int,
	f_period nvarchar(25),
	info nvarchar(max),
	link nvarchar(max) not null,
	old_code nvarchar(35),
	old_link nvarchar(max),
	img_path nvarchar(max),
	tree_code nvarchar(15),
	tree nvarchar(max),
	subgroup int references subgroups(id)
);

create table detail_complectation_s
(
	detail_code nvarchar(35) references details(detail_code),
	complectation_code nvarchar(50) references complectations(complectation_code),
	primary key(detail_code, complectation_code)
)