# ThePunch

## Visão Geral

Jogo mobile onde o player derruba inimigos, empilha-os nas costas e vende para ganhar dinheiro. O jogador pode fazer upgrades de capacidade, velocidade e skin, com sistema de coleta, pilha, upgrades, UI dinâmica e feedbacks sonoros/visuais.

## Como Rodar

1. Clone ou baixe este repositório.
2. Abra a pasta do projeto no Unity Hub.
3. Abra a cena principal em `Assets/Scenes/SampleScene.unity`.
4. Dê Play no editor ou faça build para Android/iOS.

**Build jogável:** [**The Punch**](https://fernando-farias.itch.io/the-punch)
---

## Estrutura de Pastas

- **Art/**: Artes, ícones e imagens do jogo.
- **Materials/**: Materiais para objetos e personagens.
- **Models/**: Modelos 3D.
- **Prefabs/**: Objetos prontos para reutilização (player, inimigos, moedas, UI).
- **Scenes/**: Cenas do jogo.
- **Scripts/**: Códigos do jogo, organizados por função (Player, Enemy, UI, etc).
- **Audio/**: Efeitos sonoros e músicas.
- **Animations/**: Animações dos personagens e objetos.
- **Fonts/**: Fontes usadas na UI.
- **Resources/**: Arquivos acessados dinamicamente pelo código.

---

## Mecânicas e Sistemas

### 1. **PlayerController**
- Movimento com teclado/controle ou joystick virtual.
- Soco automático em inimigos próximos (com cooldown).
- Integração com Animator para animações de andar, correr e socar.
- Sons de passos e soco sincronizados via Animation Events.
- O som do soco só toca se realmente acertar um inimigo válido.

### 2. **Sistema de Pilha (StackManager & StackableCharacter)**
- Inimigos derrotados podem ser coletados e empilhados nas costas do player.
- Capacidade máxima de empilhamento definida por upgrades.
- Efeito de inércia suave usando SmoothDamp.
- Flags:  
  - `isStacked`: se já foi empilhado.
  - `canBeCollected`: só pode ser coletado após um pequeno delay.
- Ao vender, todos os empilhados são removidos e o dinheiro é creditado.

### 3. **Sistema de Upgrades (UpgradeManager)**
- Upgrade unificado: cada nível define capacidade, velocidade, material (skin) e custo.
- Configurável via lista no Inspector (`PlayerUpgradeLevel`).
- Ao comprar upgrade:
  - Aumenta capacidade máxima.
  - Troca a skin/material do player.
  - Aumenta a velocidade de movimento.
  - Atualiza UI e barra de capacidade.
  - Toca som de upgrade.

### 4. **Barra de Capacidade (CapacityBarUI)**
- Visual composta por duas barras (fundo = capacidade máxima, frente = carregado).
- Preenchimento proporcional ao número de slots.
- Atualizada em tempo real pelo StackManager.

### 5. **Spawner Dinâmico de Inimigos (EnemySpawner)**
- Mantém sempre até o máximo de inimigos na área.
- Spawna novos conforme o player coleta/vende.
- Área de spawn configurável.

### 6. **Sistema de Áudio (MusicManager)**
- Singleton para música de fundo e efeitos sonoros.
- Métodos para tocar SFX e música, com controle de volume.
- Sons de passos, soco, venda e upgrade integrados.

### 7. **Tutorial guiado por Placa**
- Placa 3D no cenário exibe mensagens automáticas conforme o progresso do jogador.
- Tutorial orienta o jogador nos primeiros passos, upgrades e objetivos.

---

## Fluxo de Gameplay

1. **Player anda pelo cenário**.
2. **Ao se aproximar de um inimigo**, soca automaticamente (se cooldown permitir).
3. **Inimigo derrotado** entra em ragdoll e, após delay, pode ser coletado.
4. **Player coleta e empilha** inimigos até o limite de capacidade.
5. **Ao vender**, recebe dinheiro proporcional à quantidade empilhada.
6. **Pode comprar upgrades** para aumentar capacidade, velocidade e trocar skin.
7. **Spawner mantém o desafio** sempre com inimigos na área.

---

## Boas Práticas e Otimizações

- Separação clara entre lógica de jogo e UI.
- Pooling e limitação de buscas para performance.
- Uso de eventos para atualização de UI.
- Limpeza de assets/scripts não utilizados.
- Layer Collision Matrix para evitar bugs de colisão com ragdolls.
- Testado para rodar a 60fps em aparelhos atuais e 30fps em aparelhos antigos.

---

## Processo de Gestão do Projeto

- **Planejamento:** Definição das mecânicas principais, divisão do trabalho em etapas (movimento, pilha, pooling, upgrades, tutorial).
- **Execução:** Implementação incremental, testes frequentes, correção de bugs e otimização contínua.
- **Organização:** Estrutura de pastas e scripts por responsabilidade, uso de comentários e headers no Inspector.
- **Testes:** Testes em mobile e PC, ajustes de valores para garantir fluidez e responsividade.
- **Resultado:** Jogo funcional, fluido, fácil de expandir e com tutorial claro.

---

**Dúvidas sobre scripts, integração ou expansão? Consulte este README ou os scripts comentados!**

## Créditos e Licença

- Todos os assets de arte, áudio e UI utilizados neste projeto foram obtidos gratuitamente na Asset Store da Unity.
- Não possuímos informações detalhadas sobre os criadores individuais dos assets.
- Este projeto é para fins educacionais/demonstração.

