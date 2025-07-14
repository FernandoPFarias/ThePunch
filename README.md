# ThePunch

## Visão Geral

Jogo onde o player derruba inimigos, empilha-os nas costas e vende para ganhar dinheiro. O jogador pode fazer upgrades de capacidade e skin, com sistema de coleta, pilha, upgrades, UI dinâmica e feedbacks sonoros/visuais.

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
- Soco automático em inimigos próximos (com cooldown e checagem de estado).
- Integração com Animator para animações de andar, correr e socar.
- Sons de passos e soco sincronizados via Animation Events.
- O som do soco só toca se realmente acertar um inimigo válido (não ragdoll/morto).

### 2. **Sistema de Pilha (StackManager & StackableCharacter)**
- Inimigos derrotados podem ser coletados e empilhados nas costas do player.
- Capacidade máxima de empilhamento definida por upgrades.
- Só empilha inimigos realmente derrotados (em ragdoll e liberados para coleta).
- Flags:  
  - `isStacked`: se já foi empilhado.
  - `canBeCollected`: só pode ser coletado após um pequeno delay.
- Ao vender, todos os empilhados são removidos e o dinheiro é creditado.

### 3. **Sistema de Upgrades (UpgradeManager)**
- Upgrade unificado: cada nível define capacidade, material (skin) e custo.
- Configurável via lista no Inspector (`PlayerUpgradeLevel`).
- Ao comprar upgrade:
  - Aumenta capacidade máxima.
  - Troca a skin/material do player.
  - Atualiza UI e barra de capacidade.
  - Toca som de upgrade.

### 4. **Troca de Skin (PlayerSkinChanger)**
- Aplica material em todas as partes do personagem.
- Integrado ao sistema de upgrade.

### 5. **Barra de Capacidade (CapacityBarUI)**
- Visual composta por duas barras (fundo = capacidade máxima, frente = carregado).
- Preenchimento proporcional ao número de slots.
- Atualizada em tempo real pelo StackManager.

### 6. **Spawner Dinâmico de Inimigos (EnemySpawner)**
- Mantém sempre até o máximo de inimigos na área.
- Spawna novos conforme o player coleta/vende.
- Área de spawn configurável.

### 7. **Sistema de Áudio (MusicManager)**
- Singleton para música de fundo e efeitos sonoros.
- Métodos para tocar SFX e música, com controle de volume.
- Sons de passos, soco, venda e upgrade integrados.

### 8. **Animações e Avatar Mask**
- Uso de Avatar Mask e Animation Layers para permitir que o player socar enquanto anda (parte de cima soca, parte de baixo continua andando).
- Animation Events sincronizam sons e ativação da hitbox do soco.

---

## Fluxo de Gameplay

1. **Player anda pelo cenário**.
2. **Ao se aproximar de um inimigo**, soca automaticamente (se cooldown permitir).
3. **Inimigo derrotado** entra em ragdoll e, após delay, pode ser coletado.
4. **Player coleta e empilha** inimigos até o limite de capacidade.
5. **Ao vender**, recebe dinheiro proporcional à quantidade empilhada.
6. **Pode comprar upgrades** para aumentar capacidade e trocar skin.
7. **Spawner mantém o desafio** sempre com inimigos na área.

---

## Boas Práticas e Otimizações

- Separação clara entre lógica de jogo e UI.
- Pooling e limitação de buscas para performance.
- Uso de eventos para atualização de UI.
- Limpeza de assets/scripts não utilizados.
- Layer Collision Matrix para evitar bugs de colisão com ragdolls.

---

## Próximos Passos Sugeridos

- Animações suaves na barra de capacidade.
- Efeitos visuais ao empilhar/vender inimigos.
- Novos upgrades e customizações.
- Expansão do sistema de inimigos e desafios.

---

**Qualquer dúvida sobre scripts, integração ou expansão, consulte este README ou os scripts comentados!**

