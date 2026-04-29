-- База данных "Игровой клуб (PakDzal_Games)"
-- Создание таблиц и тестовых данных

-- Таблица 1: Users (Пользователи)
CREATE TABLE IF NOT EXISTS Users (
    user_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(20),
    city VARCHAR(50) NOT NULL DEFAULT 'Курск',
    registration_date DATE DEFAULT CURRENT_DATE
);

-- Таблица 2: Games (Игры)
CREATE TABLE IF NOT EXISTS Games (
    game_id SERIAL PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    genre VARCHAR(50) NOT NULL,
    price_per_hour DECIMAL(10, 2) NOT NULL,
    available BOOLEAN DEFAULT TRUE
);

-- Таблица 3: Sessions (Игровые сессии)
CREATE TABLE IF NOT EXISTS Sessions (
    session_id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL REFERENCES Users(user_id),
    game_id INTEGER NOT NULL REFERENCES Games(game_id),
    start_time TIMESTAMP NOT NULL,
    end_time TIMESTAMP,
    total_price DECIMAL(10, 2)
);

-- Тестовые данные: Пользователи из Курска (5 записей)
INSERT INTO Users (name, email, phone, city, registration_date) VALUES
('Александр Иванов', 'ivanov@mail.ru', '+7 (4712) 12-34-56', 'Курск', '2025-01-15'),
('Мария Петрова', 'petrova@mail.ru', '+7 (4712) 23-45-67', 'Курск', '2025-02-20'),
('Дмитрий Сидоров', 'sidorov@mail.ru', '+7 (4712) 34-56-78', 'Курск', '2025-03-10'),
('Елена Смирнова', 'smirnova@mail.ru', '+7 (4712) 45-67-89', 'Курск', '2025-04-05'),
('Николай Козлов', 'kozlov@mail.ru', '+7 (4712) 56-78-90', 'Курск', '2025-05-12');

-- Тестовые данные: Игры (5 записей)
INSERT INTO Games (title, genre, price_per_hour, available) VALUES
('Counter-Strike 2', 'Шутер', 150.00, TRUE),
('Dota 2', 'MOBA', 120.00, TRUE),
('Minecraft', 'Симулятор выживания', 100.00, TRUE),
('FIFA 25', 'Спортивный симулятор', 200.00, TRUE),
('Chess.com', 'Настольная игра', 80.00, TRUE);

-- Тестовые данные: Сессии (5 записей)
INSERT INTO Sessions (user_id, game_id, start_time, end_time, total_price) VALUES
(1, 1, '2025-06-01 14:00:00', '2025-06-01 17:00:00', 450.00),
(2, 2, '2025-06-02 15:00:00', '2025-06-02 18:00:00', 360.00),
(3, 3, '2025-06-03 10:00:00', '2025-06-03 14:00:00', 400.00),
(4, 4, '2025-06-04 16:00:00', '2025-06-04 19:00:00', 600.00),
(5, 5, '2025-06-05 11:00:00', '2025-06-05 13:00:00', 160.00);