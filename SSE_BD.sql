-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               12.2.2-MariaDB - MariaDB Server
-- Server OS:                    Win64
-- HeidiSQL Version:             12.14.0.7165
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- Dumping data for table sse_desktop_v1.carreras: ~3 rows (approximately)
INSERT INTO `carreras` (`id_carrera`, `clave_carrera`, `nombre_carrera`) VALUES
	(1, 'ISC', 'Ingeniería en Sistemas Computacionales'),
	(2, 'LDE', 'Licenciatura en Derecho'),
	(3, 'LCE', 'Licenciatura en Ciencias de la Educación');

-- Dumping data for table sse_desktop_v1.egresados: ~4 rows (approximately)
INSERT INTO `egresados` (`id_egresado`, `matricula`, `nombre`, `apellido_paterno`, `apellido_materno`, `curp`, `sexo`, `fecha_nacimiento`, `correo`, `telefono`, `domicilio`, `fotografia`, `id_carrera`, `id_generacion`, `fecha_egreso`, `titulado`, `estado_laboral`, `id_empresa`, `puesto`, `trabajo_relacionado`, `fecha_registro`, `activo`) VALUES
	(1, '18001ISC', 'Carlos', 'Gómez', 'Pérez', 'GOPC990101XYZABC01', 'M', '1999-01-01', 'carlos.g@email.com', '9211234567', 'Col. Centro, Coatzacoalcos', NULL, 1, 1, '2022-07-15', 1, 'Empleado', 1, 'Desarrollador de Software', 1, '2026-06-08 15:54:35', 1),
	(2, '18002ISC', 'Ana', 'Martínez', 'Ruiz', 'MARA990515XYZABC02', 'F', '1999-05-15', 'ana.m@email.com', '9219876543', 'Col. Petrolera, Minatitlán', NULL, 1, 1, '2022-07-15', 0, 'Desempleado', NULL, NULL, 0, '2026-06-08 15:54:35', 1),
	(3, '18001LDE', 'Jorge', 'López', 'Sánchez', 'LOSJ980220XYZABC03', 'M', '1998-02-20', 'jorge.l@email.com', '9214445555', 'Fracc. Paraíso, Coatzacoalcos', NULL, 2, 3, '2022-07-15', 1, 'Independiente', NULL, 'Abogado Litigante', 1, '2026-06-08 15:54:35', 1),
	(4, '19001ISC', 'Laura', 'Díaz', 'García', 'DIGL001010XYZABC04', 'F', '2000-10-10', 'laura.d@email.com', '9218889999', 'Col. Puerto México, Coatzacoalcos', NULL, 1, 2, '2023-07-15', 0, 'Posgrado', NULL, NULL, 1, '2026-06-08 15:54:35', 1);

-- Dumping data for table sse_desktop_v1.empresas: ~3 rows (approximately)
INSERT INTO `empresas` (`id_empresa`, `nombre`, `sector`, `ubicacion`) VALUES
	(1, 'Tech Solutions del Golfo', 'Tecnología', 'Coatzacoalcos, Veracruz'),
	(2, 'Despacho Jurídico Asociados', 'Servicios Legales', 'Minatitlán, Veracruz'),
	(3, 'Colegio Bilingüe Integral', 'Educación', 'Coatzacoalcos, Veracruz');

-- Dumping data for table sse_desktop_v1.encuestas: ~1 rows (approximately)
INSERT INTO `encuestas` (`id_encuesta`, `nombre_encuesta`, `estado`) VALUES
	(1, 'Encuesta de Seguimiento a Egresados 2026', 'activa');

-- Dumping data for table sse_desktop_v1.generaciones: ~3 rows (approximately)
INSERT INTO `generaciones` (`id_generacion`, `año_ingreso`, `año_egreso`, `id_carrera`) VALUES
	(1, '2018', '2022', 1),
	(2, '2019', '2023', 1),
	(3, '2018', '2022', 2);

-- Dumping data for table sse_desktop_v1.historial_actualizaciones: ~1 rows (approximately)
INSERT INTO `historial_actualizaciones` (`id_historial`, `id_egresado`, `id_usuario`, `campo_modificado`, `valor_anterior`, `valor_nuevo`, `fecha_modificacion`) VALUES
	(1, 2, 3, 'telefono', '9210000000', '9219876543', '2026-06-08 15:54:35');

-- Dumping data for table sse_desktop_v1.modalidades_titulacion: ~4 rows (approximately)
INSERT INTO `modalidades_titulacion` (`id_modalidad`, `nombre_modalidad`, `activo`) VALUES
	(1, 'Titulación por Promedio', 1),
	(2, 'Tesis', 1),
	(3, 'Examen General de Conocimientos (EGEL u equivalente)', 1),
	(4, 'Experiencia Profesional', 1);

-- Dumping data for table sse_desktop_v1.preguntas_encuesta: ~4 rows (approximately)
INSERT INTO `preguntas_encuesta` (`id_pregunta`, `id_encuesta`, `texto_pregunta`, `tipo_pregunta`) VALUES
	(1, 1, '¿Actualmente se encuentra trabajando?', 'opcion_multiple'),
	(2, 1, '¿En qué sector económico trabaja actualmente?', 'opcion_multiple'),
	(3, 1, '¿Qué tan satisfecho está con la formación académica recibida en la institución?', 'escala'),
	(4, 1, '¿Tiene algún comentario, sugerencia o retroalimentación para la institución?', 'abierta');

-- Dumping data for table sse_desktop_v1.respuestas_encuesta: ~4 rows (approximately)
INSERT INTO `respuestas_encuesta` (`id_respuesta`, `id_egresado`, `id_pregunta`, `respuesta_texto`) VALUES
	(1, 1, 1, 'Sí'),
	(2, 1, 2, 'Tecnología de la Información'),
	(3, 1, 3, '5'),
	(4, 1, 4, 'Me gustaría que agregaran más prácticas de desarrollo web moderno en el plan de estudios.');

-- Dumping data for table sse_desktop_v1.roles: ~4 rows (approximately)
INSERT INTO `roles` (`id_rol`, `nombre_rol`) VALUES
	(1, 'Administrador'),
	(2, 'Coordinador Académico'),
	(3, 'Capturista'),
	(4, 'Consultor (Solo Lectura)');

-- Dumping data for table sse_desktop_v1.titulaciones: ~2 rows (approximately)
INSERT INTO `titulaciones` (`id_titulacion`, `id_egresado`, `id_modalidad`, `fecha_titulacion`, `num_acta`, `observaciones`, `fecha_registro`) VALUES
	(1, 1, 1, '2023-02-10', 'ACTA-ISC-001', 'Aprobado por unanimidad mediante excelencia académica.', '2026-06-08 15:54:35'),
	(2, 3, 2, '2024-05-20', 'ACTA-LDE-045', 'Defensa de tesis aprobada con mención honorífica.', '2026-06-08 15:54:35');

-- Dumping data for table sse_desktop_v1.usuarios: ~3 rows (approximately)
INSERT INTO `usuarios` (`id_usuario`, `username`, `password_hash`, `id_rol`) VALUES
	(1, 'admin_sistema', '$2y$10$dummyHashDePrueba1234567890abcdef', 1),
	(2, 'coord_sistemas', '$2y$10$dummyHashDePrueba0987654321fedcba', 2),
	(3, 'capturista_01', '$2y$10$dummyHashDePrueba1122334455xyzxyz', 3);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
