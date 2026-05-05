-- Plano de Contas Padrão Simples
-- Estrutura hierárquica com 2 níveis

-- 1 ATIVO (Raiz)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (NULL, 'A', '1', 'ATIVO');

-- 2 Ativo Circulante (Filho de ATIVO)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (LAST_INSERT_ID(), 'A', '1.1', 'ATIVO CIRCULANTE');

-- 3 Ativo Não Circulante (Filho de ATIVO)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (1, 'A', '1.2', 'ATIVO NAO CIRCULANTE');

-- 4 PASSIVO (Raiz)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (NULL, 'P', '2', 'PASSIVO');

-- 5 Passivo Circulante (Filho de PASSIVO)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (LAST_INSERT_ID(), 'P', '2.1', 'PASSIVO CIRCULANTE');

-- 6 Passivo Não Circulante (Filho de PASSIVO)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (4, 'P', '2.2', 'PASSIVO NAO CIRCULANTE');

-- 7 PATRIMÔNIO LÍQUIDO (Raiz)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (NULL, 'E', '3', 'PATRIMONIO LIQUIDO');

-- 8 RECEITAS (Raiz)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (NULL, 'R', '4', 'RECEITAS');

-- 9 Receita Operacional (Filho de RECEITAS)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (LAST_INSERT_ID(), 'R', '4.1', 'RECEITA OPERACIONAL');

-- 10 Receita Financeira (Filho de RECEITAS)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (8, 'R', '4.2', 'RECEITA FINANCEIRA');

-- 11 DESPESAS (Raiz)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (NULL, 'D', '5', 'DESPESAS');

-- 12 Despesa Operacional (Filho de DESPESAS)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (LAST_INSERT_ID(), 'D', '5.1', 'DESPESA OPERACIONAL');

-- 13 Despesa Financeira (Filho de DESPESAS)
INSERT INTO tb_plano_conta (CodigoPai, Tipo, Rotulo, Descricao)
VALUES (11, 'D', '5.2', 'DESPESA FINANCEIRA');
