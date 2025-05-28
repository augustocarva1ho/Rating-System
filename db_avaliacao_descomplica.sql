create database if not exists avaliacao_descomplicasp;

use avaliacao_descomplicasp;

create table unidades(
	idUni CHAR(2) NOT NULL PRIMARY KEY,
    nomeUni VARCHAR(20) NOT NULL
);
create table avaliacao(
	idAval INT AUTO_INCREMENT PRIMARY KEY,
    senhaAval VARCHAR(6) NOT NULL,
    notaAval INT NOT NULL,
    unidadeAval CHAR(2),
    dataAval TIMESTAMP DEFAULT CURRENT_TIMESTAMP,    
    FOREIGN KEY(unidadeAval) REFERENCES unidades(idUni)
);

/*Cadastrando unidades*/
INSERT INTO unidades (idUni, nomeUni)
VALUES 
('PJ', 'Pirituba/Jaraguá'),
('PA', 'Parelheiros'),
('CV', 'Casa Verde'),
('IT', 'Itaim Paulista'),
('IQ', 'Itaquera'),
('EM', 'Ermelino Matarazzo'),
('GU', 'Guaianases'),
('PI', 'Pinheiros'),
('AF', 'Aricanduva'),
('MB', 'MBoi Mirim');

SELECT * FROM avaliacao ORDER BY idAval ;

drop database avaliacao_descomplicasp;
