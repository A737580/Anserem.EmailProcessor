# Anserem.EmailProcessor

**Anserem.EmailProcessor** — консольное кроссплатформенное приложение на **.NET 10**, реализующее бизнес-правила заполнения и фильтрации поля `Copy` у email-писем в соответствии с тестовым заданием компании **Anserem** (позиция: *Разработчик .NET & JS*).

Проект реализован с упором на чистую архитектуру, SOLID-принципы и покрытие бизнес-логики unit-тестами.

---

## Описание задачи

Email-сообщение содержит поля:

```txt
From
To
Copy
BlindCopy
Title
Body
````

Существует:

* список доменов, подпадающих под бизнес-правило,
* список адресов для подстановки (по доменам),
* список исключений (по доменам).

### Бизнес-логика

1. Если в `To` или `Copy` есть хотя бы один email с доменом из списка доменов:

   * В `Copy` добавляются адреса для подстановки соответствующего домена.
2. Если при этом найден адрес из списка исключений:

   * Подстановка **не происходит**.
   * Из `Copy` удаляются все адреса для подстановки данного домена.
3. Дублирование адресов не допускается.
4. Формат:

   * Один email → `mail@domain.com`
   * Список → `mail@domain.com; mail2@domain.com;`

---

## Стек технологий

```txt
- .NET 10
- C#
- xUnit (unit-тестирование)
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Configuration
- Docker / Docker Compose
- Linux (разработка велась на Ubuntu)
```

---

## Архитектура

Использована упрощённая Clean Architecture с разделением на слои:

```txt
Anserem.EmailProcessor/
├── Anserem.EmailProcessor.Console       # Точка входа (UI, DI)
│   ├── Interfaces
│   ├── Services
│   ├── Program.cs
│   └── appsettings.json
│
├── Anserem.EmailProcessor.Domain        # Бизнес-логика
│   ├── Interfaces
│   ├── Models
│   └── Services
│
├── Anserem.EmailProcessor.Tests         # Unit-тесты
│   ├── EmailMessageTests.cs
│   ├── EmailParserTests.cs
│   └── EmailServiceTests.cs
│
├── docker-compose.yml
├── dockerfile
└── README.md
```

### Принципы

* SOLID
* Dependency Injection
* Разделение ответственности
* Изоляция бизнес-логики от инфраструктуры
* Тестируемость

---

## Основные компоненты

### `EmailMessage`

* Модель email-сообщения
* Форматирование и хранение данных
* Валидация

### `EmailParser`

* Парсинг строкового представления email-адресов
* Валидация
* Работа со списками

### `EmailService`

* Реализация бизнес-правил
* Фильтрация
* Подстановка
* Удаление адресов при наличии исключений

---

## Тестирование

Используется **xUnit**.

Покрытие тестами:

```txt
EmailMessage
EmailParser
EmailService
```

Тесты проверяют:

* корректность подстановки
* работу исключений
* отсутствие дублирования
* корректное удаление адресов
* граничные случаи
* форматирование

### Важно

Тесты включены в Docker-сборку.

Если хотя бы один тест не проходит — контейнер **не будет собран**.

Если контейнер запущен — значит все тесты успешно прошли.

Посмотреть лог выполнения тестов можно через:

```bash
docker compose build --no-cache --progress=plain
```

---

## Запуск через Docker

### Сборка

```bash
docker compose build
```

### Запуск

```bash
docker compose run --rm email-processor
```

### Остановка

```bash
docker compose down
```

---

## Примеры бизнес-логики

### Добавление адреса (есть домен, нет исключений)

**Вход:**

```
To: q.qweshnikov@batut.com; w.petrov@alfa.com;
Copy: f.patit@buisness.com;
```

**Выход:**

```
To: q.qweshnikov@batut.com; w.petrov@alfa.com;
Copy: f.patit@buisness.com; v.vladislavovich@alfa.com;
```

---

### Не добавляется (есть исключение)

**Вход:**

```
To: t.kogni@acl.com
Copy: i.ivanov@tbank.ru
```

**Выход:**

```
To: t.kogni@acl.com
Copy: i.ivanov@tbank.ru
```

---

### Удаление адресов при исключении

**Вход:**

```
To: t.kogni@acl.com; i.ivanov@tbank.ru
Copy: e.gras@tbank.ru; t.tbankovich@tbank.ru; v.veronickovna@tbank.ru
```

**Выход:**

```
To: t.kogni@acl.com; i.ivanov@tbank.ru
Copy: e.gras@tbank.ru
```

---

## Валидация и обработка ошибок

* Проверка формата email
* Корректная работа с пустыми значениями
* Защита от дублирования
* Обработка некорректного ввода
* Чистая обработка исключений

---

## Используемые NuGet-пакеты

### Console-проект

```xml
Microsoft.Extensions.Configuration.Json
Microsoft.Extensions.DependencyInjection
Microsoft.Extensions.Options.ConfigurationExtensions
```

### Domain-проект

```xml
Microsoft.Extensions.Options
```

---

## Особенности реализации

* Кроссплатформенность (.NET 10)
* Docker-окружение
* Тесты как часть CI-логики сборки
* Чистая структура проекта
* Расширяемость (можно легко добавить веб-слой или API)

---

## Возможные улучшения

```txt
[ ] Добавить GitHub Actions (CI)
[ ] Реализовать Web API-обёртку
[ ] Добавить логирование через Serilog
[ ] Повысить покрытие тестами (edge cases)
[ ] Добавить интеграционные тесты
```
