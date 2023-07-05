# Sistema de Gestão de Torneios Left 4 Dead 2

[![Status da Construção](https://dev.azure.com/altairsossai/Torneio%20L4D2/_apis/build/status/build-azure-functions?branchName=master)](https://dev.azure.com/altairsossai/Torneio%20L4D2/_build/latest?definitionId=21&branchName=master)
[![Status do Lançamento](https://vsrm.dev.azure.com/altairsossai/_apis/public/Release/badge/59d9b252-ba33-43e4-b3c5-b8b204c4dfae/1/1)](https://dev.azure.com/altairsossai/Torneio%20L4D2/_release?_a=releases&view=mine&definitionId=1)

## Sobre o Projeto
Este projeto foi criado para apoiar a comunidade de **Left 4 Dead 2**, na qual estou participando, que organizou um torneio com o objetivo de fortalecer as amizades dentro e fora do jogo. O torneio segue um sistema de pontos corridos, semelhante aos campeonatos de futebol.

A fim de facilitar a gestão deste torneio, desenvolvi um site para controlar os confrontos, jogadores, equipes e pontuações. A aplicação é dividida em duas partes: um *front-end* feito em **Angular** e um *back-end* desenvolvido em **C#** usando *Azure Functions*. Todos os dados são armazenados em uma *Storage Account* no *Azure*.

## Organizadores do Torneio
- [Altair](https://steamcommunity.com/profiles/76561198141521946/)
- [Fear](https://steamcommunity.com/profiles/76561198135872482/)
- [Lyon](https://steamcommunity.com/profiles/76561198076227103/)

## Links da Aplicação
A aplicação está disponível nos seguintes endereços:

- Site do Torneio: https://torneio-l4d2.azurewebsites.net/
- Código Front-end: https://github.com/altair-sossai/torneio-l4d2-front-end
- Código Back-end: https://github.com/altair-sossai/torneio-l4d2-back-end

## Recursos da Aplicação
A aplicação oferece os seguintes recursos:

- Recursos Administrativos:
  - Cadastro de jogadores através do perfil da Steam.
  - Cadastro de equipes.
  - Vinculação dos jogadores às equipes.
  - Geração automática dos confrontos.
  - Cadastro dos resultados dos confrontos.
  
- Recursos Públicos:
  - Acompanhamento da tabela geral de pontos.
  - Visualização do resultado dos confrontos.
  - Visualização das equipes e jogadores participantes.

## Capturas de Tela da Aplicação
- Tela administrativa para gerenciar os jogadores no torneio.<br/><br/>
![Cadastro dos jogadores](https://torneiol4d2.blob.core.windows.net/imgs/cadastro-jogadores.png)<br/><br/>

- Tela administrativa para gerenciar as equipes e seus jogadores.<br/><br/>
![Cadastro das equipes](https://torneiol4d2.blob.core.windows.net/imgs/cadastro-times.png)<br/><br/>

- Tela administrativa para gerenciar os confrontos entre as equipes, pontuação e penalidades.<br/><br/>
![Cadastro dos confrontos](https://torneiol4d2.blob.core.windows.net/imgs/cadastro-confrontos.png)<br/><br/>

- Tela pública para acompanhar os confrontos.<br/><br/>
![Visualização dos confrontos](https://torneiol4d2.blob.core.windows.net/imgs/visualizacao-confrontos.png)<br/><br/>

- Tela pública para acompanhar a pontuação geral do torneio.<br/><br/>
![Tabela de pontuação geral](https://torneiol4d2.blob.core.windows.net/imgs/tabela-pontos-gerais.png)<br/><br/>
