-- ======================================
-- INIT SCRIPT - ECOMMERCE DE BEBIDAS (ESQUEMA GENÉRICO)
-- ======================================

-- ======================================
-- 1. DATOS INICIALES - CATEGORIAS DE BEBIDAS
-- ======================================
INSERT INTO categorias (nombre, descripcion) VALUES
('Vinos Tintos', 'Malbec, Cabernet Sauvignon, Merlot y blends'),
('Vinos Blancos y Rosados', 'Chardonnay, Sauvignon Blanc, Torrontés y rosados dulces'),
('Cervezas Artesanales', 'IPA, APA, Stout, Porter y cervezas de especialidad'),
('Whiskys y Bourbons', 'Single Malts, Blends escoceses y bourbons americanos'),
('Espumantes', 'Champagne, Extra Brut, Brut Nature y Demi Sec'),
('Gines y Destilados', 'Gin botánico, Vodka premium y Tequila'),
('Aperitivos y Licores', 'Fernet, Campari, Vermouth y licores dulces'),
('Cristalería y Accesorios', 'Copas, vasos, cocteleras y destapadores');

-- ======================================
-- 2. PRODUCTOS DE BEBIDAS DE EJEMPLO (Sin columna 'genero')
-- ======================================
INSERT INTO productos (sku, nombre, descripcion, precio_base, descuento, peso, categoria_id, imagen_principal, es_nuevo, es_destacado, meta_title, meta_description) VALUES
-- Vinos
('VIN-MAL-RES-01', 'Vino Tinto Malbec Reserva "Finca Las Moras"', 'Un Malbec de color rojo violáceo profundo. En nariz presenta aromas a frutos rojos maduros como ciruelas y cerezas, con notas de vainilla y chocolate aportadas por su paso de 12 meses en barrica de roble francés. En boca es de cuerpo medio, taninos suaves y final persistente. Ideal para acompañar carnes rojas asadas y pastas con salsas intensas.', 15000.00, 10.00, 1.2, 1, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764199252/mi_ecommerce_productos/vino_malbec.jpg', FALSE, TRUE, 'Vino Malbec Reserva - Finca Las Moras', 'Vino tinto Malbec reserva con 12 meses en barrica. Excelente relación calidad-precio.'),
('VIN-BLA-TOR-02', 'Vino Torrontés "Cafayate Estate"', 'Vino blanco joven y frutado. Destaca por sus intensos aromas florales a jazmín y notas cítricas. En boca es fresco, equilibrado y con una acidez vibrante. Perfecto para maridar con comida picante, empanadas salteñas o platos de la cocina asiática.', 9500.00, 0.00, 1.2, 2, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764200784/mi_ecommerce_productos/vino_torrontes.jpg', TRUE, FALSE, 'Vino Blanco Torrontes - Valle de Cafayate', 'Vino blanco fresco y aromático, ideal para días de verano.'),

-- Cervezas
('CER-IPA-LAT-01', 'Cerveza Patagonia IPA "24.7" - Lata', 'Cerveza de estilo Session IPA, muy refrescante y de amargor marcado pero amigable. Elaborada con lúpulo patagónico y flor de sauco. Presenta notas cítricas y frutales intensas. Color dorado brillante y espuma persistente.', 2500.00, 0.00, 0.5, 3, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764200669/mi_ecommerce_productos/cerveza_ipa.jpg', FALSE, TRUE, 'Cerveza Patagonia IPA 24.7 en Lata', 'Cerveza artesanal Session IPA con notas a flor de sauco. Refrescante y aromática.'),

-- Whiskys y Destilados
('WHI-ESC-12A-01', 'Whisky Johnnie Walker Black Label 12 Años', 'El icónico blend escocés. Una mezcla de whiskies de malta y grano de toda Escocia, añejados por un mínimo de 12 años. Perfil de sabor complejo con notas a frutos negros, vainilla dulce y el característico toque ahumado final. Un clásico indiscutido para disfrutar solo o con hielo.', 85000.00, 5.00, 1.5, 4, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764200585/mi_ecommerce_productos/whisky_black.jpg', FALSE, TRUE, 'Whisky Johnnie Walker Black Label', 'Whisky escoces de 12 años. Sabor ahumado y complejo. Envíos a todo el país.'),
('GIN-PRE-BOT-01', 'Gin "Aconcagua" London Dry', 'Gin de autor nacional elaborado en alambique de cobre. Destacan sus notas botánicas donde el enebro patagónico toma protagonismo, acompañado de coriandro, raíz de angélica y un toque cítrico de piel de limón. Ideal para la preparación del clásico Gin Tonic.', 22000.00, 0.00, 1.3, 6, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764200213/mi_ecommerce_productos/gin_aconcagua.jpg', TRUE, TRUE, 'Gin Aconcagua Premium London Dry', 'Gin nacional de alta calidad, botánicos seleccionados a mano.'),

-- Aperitivos
('FER-CLA-BRN-01', 'Fernet Branca Clásico', 'El aperitivo por excelencia de las previas argentinas. Elaborado con una receta secreta de decenas de hierbas y especias de cuatro continentes. Reposado en cubas de roble durante un año. Su sabor es inconfundible: amargo, herbáceo y con carácter.', 18000.00, 15.00, 1.4, 7, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764200414/mi_ecommerce_productos/fernet_branca.jpg', FALSE, TRUE, 'Fernet Branca Clasico - Botella 750ml', 'El aperitivo amargo clásico, indispensable para cualquier juntada.'),
('CAM-CLA-MIL-01', 'Aperitivo Campari', 'Aperitivo italiano de color rojo brillante y sabor único, obtenido de la infusión de hierbas amargas, plantas aromáticas y frutas en alcohol y agua. Ingrediente fundamental para cócteles icónicos como el Negroni y el Americano.', 14500.00, 0.00, 1.4, 7, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764445176/mi_ecommerce_productos/campari.jpg', FALSE, FALSE, 'Campari Aperitivo Clasico', 'Aperitivo italiano de sabor inconfundible. Base para el Negroni perfecto.');


-- ======================================
-- 3. VARIANTES DE PRODUCTOS (VOLUMEN/TAMAÑO)
-- (Usamos la columna "atributo_variante" en lugar de "talle")
-- ======================================

-- Vino Tinto Malbec (ID: 1)
INSERT INTO variantes_producto (producto_id, atributo_variante, sku_variante, stock) VALUES
(1, '750ml', 'VIN-MAL-RES-750', 50),
(1, '1.5L', 'VIN-MAL-RES-1500', 15); -- Formato Magnum

-- Vino Torrontés (ID: 2)
INSERT INTO variantes_producto (producto_id, atributo_variante, sku_variante, stock) VALUES
(2, '750ml', 'VIN-BLA-TOR-750', 30);

-- Cerveza Patagonia IPA (ID: 3)
INSERT INTO variantes_producto (producto_id, atributo_variante, sku_variante, stock) VALUES
(3, '473ml', 'CER-IPA-LAT-473', 120),
(3, '710ml', 'CER-IPA-LAT-710', 45);

-- Whisky Black Label (ID: 4)
INSERT INTO variantes_producto (producto_id, atributo_variante, sku_variante, stock) VALUES
(4, '750ml', 'WHI-ESC-12A-750', 25),
(4, '1L', 'WHI-ESC-12A-1000', 10);

-- Gin Aconcagua (ID: 5)
INSERT INTO variantes_producto (producto_id, atributo_variante, sku_variante, stock) VALUES
(5, '700ml', 'GIN-PRE-BOT-700', 40);

-- Fernet Branca (ID: 6)
INSERT INTO variantes_producto (producto_id, atributo_variante, sku_variante, stock) VALUES
(6, '750ml', 'FER-CLA-BRN-750', 100),
(6, '1L', 'FER-CLA-BRN-1000', 80);

-- Campari (ID: 7)
INSERT INTO variantes_producto (producto_id, atributo_variante, sku_variante, stock) VALUES
(7, '750ml', 'CAM-CLA-MIL-750', 60);


-- ======================================
-- 4. IMAGENES ADICIONALES PARA PRODUCTOS
-- ======================================
INSERT INTO producto_imagenes (producto_id, imagen, alt_text, orden) VALUES
(1, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764199257/mi_ecommerce_productos/vino_malbec_copa.jpg', 'Vino Malbec servido en copa', 1),
(3, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764200414/mi_ecommerce_productos/cerveza_ipa_vaso.jpg', 'Cerveza IPA en vaso pinta', 1),
(4, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764200589/mi_ecommerce_productos/whisky_caja.jpg', 'Whisky Black Label con estuche', 1),
(6, 'https://res.cloudinary.com/dplnmknty/image/upload/v1764200419/mi_ecommerce_productos/fernet_cola.jpg', 'Trago preparado de Fernet con cola', 1);


-- ======================================
-- 5. USUARIOS DE EJEMPLO
-- ======================================
INSERT INTO usuarios (nombre, email, password, rol, telefono) VALUES
('Administrador', 'admin@tiendabebidas.com', '$2a$12$M6tl8zr7rih5HJPj.9lMjedKweScAWTYgWhOHKacIvlxEFT7CjBYC', 'admin', '+54 11 1234-5678'),
('Laura Martinez', 'laura.martinez@email.com', '$2b$10$K7L/8Y1t40zH2G.B3/4iFOJTKfz.1J2M8X5W6Y0Z1A2B3C4D5E6F7G', 'usuario', '+54 11 9876-5432'),
('Carlos Gomez', 'carlos.gomez@email.com', '$2b$10$K7L/8Y1t40zH2G.B3/4iFOJTKfz.1J2M8X5W6Y0Z1A2B3C4D5E6F7G', 'usuario', '+54 11 5555-1234'),
('Sofia Rodriguez', 'sofia.rodriguez@email.com', '$2b$10$K7L/8Y1t40zH2G.B3/4iFOJTKfz.1J2M8X5W6Y0Z1A2B3C4D5E6F7G', 'usuario', '+54 9 11 7777-8888'),
('Ana Fernandez', 'ana.fernandez@email.com', '$2b$10$K7L/8Y1t40zH2G.B3/4iFOJTKfz.1J2M8X5W6Y0Z1A2B3C4D5E6F7G', 'usuario', '+54 11 4444-9999');

-- ======================================
-- 6. DIRECCIONES DE EJEMPLO
-- ======================================
INSERT INTO direcciones (usuario_id, calle, numero, piso, dpto, ciudad, provincia, codigo_postal, es_principal) VALUES
(2, 'Av. Corrientes', '1234', '5', 'B', 'Buenos Aires', 'Buenos Aires', '1043', TRUE),
(2, 'Av. Santa Fe', '5678', NULL, NULL, 'Buenos Aires', 'Buenos Aires', '1425', FALSE),
(3, 'Calle San Martin', '890', '2', 'A', 'Rosario', 'Santa Fe', '2000', TRUE),
(4, 'Av. Independencia', '456', '10', '15', 'Cordoba', 'Cordoba', '5000', TRUE),
(5, 'Calle Rivadavia', '2345', '3', 'C', 'Buenos Aires', 'Buenos Aires', '1033', TRUE);

-- ======================================
-- 7. CUPONES DE DESCUENTO
-- ======================================
INSERT INTO cupones (codigo, descripcion, tipo, valor, monto_minimo, usos_maximos, fecha_inicio, fecha_fin) VALUES
('BIENVENIDA15', 'Descuento de bienvenida para nuevos usuarios', 'porcentaje', 15.00, 10000.00, 100, '2024-01-01', '2025-12-31'),
('FIESTAS25', 'Descuento especial fin de año en espumantes y destilados', 'porcentaje', 20.00, 20000.00, 500, '2024-12-01', '2025-01-15'),
('ENVIOGRATIS', 'Envio gratis en compras mayores a $25000', 'monto_fijo', 3500.00, 25000.00, NULL, '2024-01-01', '2025-12-31'),
('VINOS25', 'Descuento especial en selección de vinos', 'porcentaje', 25.00, 15000.00, 200, '2024-01-01', '2025-06-30');

-- ======================================
-- 8. PEDIDO DE EJEMPLO
-- ======================================
INSERT INTO pedidos (numero_pedido, usuario_id, direccion_id, total, estado, notas, shipping_cost) VALUES
('PED-20241210-000001', 2, 1, 33200.00, 'entregado', 'Avisar antes de entregar. Paquete frágil.', 0.00);

-- Detalles del pedido (Cambio de "talle" a "atributo_variante")
INSERT INTO detalles_pedido (pedido_id, variante_id, sku_variante, nombre_producto, atributo_variante, cantidad, precio_unitario, descuento_aplicado) VALUES
(1, 1, 'VIN-MAL-RES-750', 'Vino Tinto Malbec Reserva', '750ml', 2, 15000.00, 10.00), -- 2 vinos con 10% de dto
(1, 8, 'FER-CLA-BRN-750', 'Fernet Branca Clásico', '750ml', 1, 18000.00, 15.00);  -- 1 fernet con 15% dto

-- Pago del pedido
INSERT INTO pagos (pedido_id, metodo, estado, transaccion_id, monto, detalle_metodo, cuotas, fecha_aprobacion) VALUES
(1, 'mercadopago', 'aprobado', 'MP-1234567890', 33200.00, 'Visa Banco Nacion - 3 cuotas sin interes', 3, '2024-12-10 14:30:00');

-- ======================================
-- 9. SUSCRIPCIONES AL NEWSLETTER
-- ======================================
INSERT INTO suscripciones (email) VALUES
('bodega_fan1@email.com'),
('cervecero_pro@email.com'),
('cocteles_encasa@email.com'),
('laura.martinez@email.com'),
('carlos.gomez@email.com'),
('ana.fernandez@email.com');

-- ======================================
-- 10. ELEMENTOS EN CARRITO (USA VARIANTE_ID)
-- ======================================
INSERT INTO carritos (usuario_id, variante_id, cantidad) VALUES
(3, 4, 6),   -- Carlos tiene 6 Latas de Cerveza IPA (473ml)
(3, 10, 2),  -- Carlos tiene 2 Fernet Branca de 1L
(4, 6, 1),   -- Sofia tiene 1 Whisky Black Label (750ml)
(5, 7, 1),   -- Ana tiene 1 Gin Aconcagua
(5, 11, 2);  -- Ana tiene 2 Campari

-- ======================================
-- 11. LOGS DE EJEMPLO
-- ======================================
INSERT INTO shipping_logs (pedido_id, proveedor, accion, request, response, estado, mensaje) VALUES
(1, 'andreani', 'cotizar', '{"origen":"1043","destino":"1043","peso":3.8,"volumen":0.04}', '{"costo":0,"tiempo_entrega":"24-48hs","servicio":"Estandar - Especial Cargas Líquidas"}', 'success', 'Cotizacion exitosa - Envio gratis por promoción'),
(1, 'andreani', 'crear_envio', '{"pedido":"PED-20241210-000001","destinatario":"Laura Martinez"}', '{"tracking":"AND123456789AR","label_url":"https://..."}', 'success', 'Envio creado correctamente');

-- ======================================
-- 12. MENSAJE DE CONFIRMACION
-- ======================================
SELECT 
    'Base de datos de bebidas inicializada correctamente' as mensaje,
    (SELECT COUNT(*) FROM categorias) as categorias_creadas,
    (SELECT COUNT(*) FROM productos) as productos_creados,
    (SELECT COUNT(*) FROM variantes_producto) as variantes_creadas,
    (SELECT COUNT(*) FROM usuarios) as usuarios_creados,
    (SELECT COUNT(*) FROM cupones) as cupones_creados,
    (SELECT SUM(stock) FROM variantes_producto) as stock_total;