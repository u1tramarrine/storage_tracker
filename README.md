# Справочники: Предметы и Ящики

**ФИО:** Шапиро Марина Михайловна
**Год:** 2026  
**Курс:**  3
**Группа:** КТС-2  

---
# Home Storage Tracker

Приложение для отслеживания предметов, которые хранятся в ящиках дома.

-Язык: С#
-Приложение: WPF desktop application
-База данных: PostgreSQL

Ссылка на запись работы приложения на YouTube:
https://youtu.be/n50u01qgydQ
---

## Структура данных

### Boxes — ящики

Физический ящик для хранения вещей.

| Field | Type | Описание |
|---|---|---|
| id | UUID | Уникальный идентификатор |
| name | string | Короткое название, например "Ящик №3" |
| location | string | Где стоит ящик, например "Кухня, верхняя полка" |
| description | string | Заметки: цвет, размер, материал |
| photo_url | string | Путь к фото ящика |
| category_id | UUID | Ссылка на Categories |

---

### Items — предметы

Предмет или набор одинаковых предметов, лежащих в ящике.

| Field | Type | Описание |
|---|---|---|
| id | UUID | Уникальный идентификатор |
| name | string | Название предмета |
| description | string | Дополнительные сведения о предмете |
| quantity | integer | Количество единиц |
| price | decimal(6,2) | Стоимость предмета |
| photo_url | string | Путь к фото предмета |
| box_id | UUID | Ссылка на Boxes |
| category_id | UUID | Ссылка на Categories |
| notes | string | Доп. информация, например "в пакете", "нижний слой" |
| created_at | date | Дата добавления записи |

---

### Categories — категории

Общий справочник категорий, используется и для ящиков, и для предметов.

| Field | Type | Описание |
|---|---|---|
| id | UUID | Уникальный идентификатор |
| name | string | Название категории, например "Инструменты", "Документы" |
| description | string | Уточнение, что относится к этой категории |

## Связь между справочниками
 
 
- Каждый **Item** принадлежит одному **Box** — связь через поле `box_id`
- Каждый **Item** может относиться к одной **Category** — связь через поле `category_id`
- Каждый **Box** может относиться к одной **Category** — связь через поле `category_id`
- Одна **Category** может быть привязана ко многим ящикам и предметам
---
 
## Типы данных
 
| Тип | Примеры полей |
|---|---|
| integer | quantity, box_id, category_id |
| UUID | id |
| string | name, description, location, notes, photo_url |
| decimal | price |
| date | created_at |

## Организация базы данных
- Используется PostgreSQL в качестве СУБД
- Схема БД. ERD диаграмма
<img width="1256" height="1129" alt="Image" src="https://github.com/user-attachments/assets/e305cda7-ec1e-4f38-ac9a-95341a46f77d" />
  

## Заполнение таблиц
Пример SQL запроса добавления данных
<img width="1995" height="165" alt="Image" src="https://github.com/user-attachments/assets/c67600d7-3cf8-4c7d-8a13-8ffe38b352b1" />
Таблица Boxes
<img width="2242" height="235" alt="Image" src="https://github.com/user-attachments/assets/301075da-9ac4-4a18-bb00-10f5bfe1e771" />
Таблица Items
<img width="2231" height="241" alt="Image" src="https://github.com/user-attachments/assets/8ad36858-658a-46f1-bff0-9d3216c8dceb" />
Таблица Categories
<img width="2300" height="218" alt="Image" src="https://github.com/user-attachments/assets/554e7f73-a1eb-4500-88ce-a347d761fddc" />
