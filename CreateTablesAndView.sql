create table models								-- создание таблицы моделей
(		
	model_code nvarchar(35) primary key,		
	f_period nvarchar(25),						-- тут можно разбит на 2 даты, 
												-- 1(not null) - начало производства, 
												-- 2 - завершение
	model_name nvarchar(50) not null,		
	complectation_cipher nvarchar(50)		
);		
		
create table complectations 					-- создание таблицы комплектаций
(
	complectation_code nvarchar(50) primary key,
	model_code nvarchar(35),
	f_period nvarchar(25),
	params nvarchar(max) not null,
	foreign key (model_code) references models(model_code) -- внешний ключ
);

create table subgroups							-- создание таблицы подгрупп						
(											
	id int primary key identity,				-- идентификатор для облегчения запровов и
												-- компактного хранения в БД,
												-- а так же для упрощения запросов (автоинкремент)
	name nvarchar(250) not null,				-- на случай очень большого значения группы						
	group_name nvarchar(250) not null,			-- использовал т.к. не работало (max) в UNIQUE						
	UNIQUE (name, group_name)					-- составной потенциальный ключ				
);

create table details							-- создание таблицы деталей
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
	subgroup int references subgroups(id)		-- внешний ключ
);

create table detail_complectation_s				-- создание таблицы для связи M-to-M между деталями и комплектациями
(
	detail_code nvarchar(35) references details(detail_code),
	complectation_code nvarchar(50) references complectations(complectation_code),
	primary key(detail_code, complectation_code)
);

CREATE VIEW result AS (
select models.model_code, models.f_period AS model_period, models.model_name, models.complectation_cipher, -- Модель
		complectations.complectation_code, complectations.f_period AS complectation_period,					-- Комплектация
		details.detail_code, details.count AS det_count, details.f_period AS detail_period,					-- Деталь
		details.info, details.link,	details.old_code, details.old_link, details.img_path, 
		details.tree_code, details.tree, 
		subgroups.name, subgroups.group_name																-- Группа
		from
		models join complectations on models.model_code = complectations.model_code 
				join detail_complectation_s on complectations.complectation_code = detail_complectation_s.complectation_code
				join details on detail_complectation_s.detail_code = details.detail_code
				join subgroups on details.subgroup = subgroups.id
);


CREATE VIEW result_full AS (
select models.model_code, models.f_period AS model_period, models.model_name, models.complectation_cipher, -- Модель
		complectations.complectation_code, complectations.f_period AS complectation_period,					-- Комплектация
		details.detail_code, details.count AS det_count, details.f_period AS detail_period,					-- Деталь
		details.info, details.link,	details.old_code, details.old_link, details.img_path, 
		details.tree_code, details.tree, 
		subgroups.name, subgroups.group_name																-- Группа
		from
		models left join complectations on models.model_code = complectations.model_code 
				left join detail_complectation_s on complectations.complectation_code = detail_complectation_s.complectation_code
				right join details on detail_complectation_s.detail_code = details.detail_code
				full outer join subgroups on details.subgroup = subgroups.id
);