INSERT INTO Buyers (IdentityNumber, Role, Name, Password, Email, Phone) VALUES
('123456789', 0, 'Noam Levi',    'pass123', 'noam@example.com',   '0501111111'),
('987654321', 1, 'Dana Cohen',   'admin1',  'dana@example.com',   '0502222222'),
('111222333', 0, 'Eyal Katz',    'pass234', 'eyal@example.com',   '0503333333'),
('444555666', 0, 'Maya Rosen',   'pass345', 'maya@example.com',   '0504444444'),
('777888999', 0, 'Amit Bar',     'pass456', 'amit@example.com',   '0505555555');


INSERT INTO Donors (Name, Email, Phone) VALUES
('David Green',   'david@example.com',  '0521111111'),
('Shira Blau',    'shira@example.com',  '0522222222'),
('Ron Adler',     'ron@example.com',    '0523333333'),
('Yael Stein',    'yael@example.com',   '0524444444'),
('Tom Segal',     'tom@example.com',    '0525555555');


INSERT INTO Gifts (Name, Description, DonorId, ImageUrl, Category, Price) VALUES
('Bluetooth Speaker', 'Portable speaker', 1, NULL, 'Electronics', 120),
('Book Voucher',      'Gift card',        6, NULL, 'Books',        80),
('Coffee Machine',    'Home coffee maker',3, NULL, 'Home',        350),
('Headphones',        'Wireless headset', 4, NULL, 'Electronics', 200),
('Board Game',        'Family fun game',  5, NULL, 'Games',        90);

INSERT INTO Orders (BuyerId, OrderDate, Status) VALUES
(1, GETDATE(), 0),
(2, GETDATE(), 1),
(3, GETDATE(), 0),
(4, GETDATE(), 1),
(5, GETDATE(), 0);


INSERT INTO OrderGift (OrderId, GiftId, IsWinner) VALUES
(1, 1, 0),
(1, 6, 1),
(2, 2, 0),
(3, 7, 0),
(4, 8, 1);

