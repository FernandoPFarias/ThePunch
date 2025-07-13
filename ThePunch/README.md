# Organização do Projeto

Este projeto está organizado em várias pastas para facilitar o trabalho e encontrar os arquivos rapidamente. Veja para que serve cada uma:

- **Art/**: Imagens, ícones e artes do jogo.
- **Materials/**: Materiais usados para dar cor e textura nos objetos.
- **Models/**: Modelos 3D dos personagens e objetos.
- **Prefabs/**: Objetos prontos para usar várias vezes, como o personagem, inimigos e moedas.
- **Scenes/**: Cenas do jogo, como o menu ou a fase principal.
- **Scripts/**: Códigos que fazem o jogo funcionar, separados por tipo (personagem, inimigo, sistema de pilha, dinheiro, melhorias e interface).
- **Audio/**: Sons e músicas do jogo.
- **Animations/**: Animações dos personagens e objetos.
- **Fonts/**: Fontes usadas nos textos do jogo.
- **Resources/**: Arquivos que podem ser carregados pelo código durante o jogo.

Assim fica mais fácil de se organizar e encontrar o que precisa durante o desenvolvimento!

---

## Resumo do progresso (última sessão)

### Sistema de Barra de Capacidade Visual
- Implementação de uma barra de capacidade composta por duas imagens do tipo Filled:
  - **Barra escura (fundo):** representa a capacidade máxima desbloqueada pelo jogador.
  - **Barra clara (frente):** representa a quantidade de inimigos atualmente carregados.
- O preenchimento de ambas as barras é proporcional ao número de divisões (slots) configurado.
- A barra clara nunca ultrapassa a barra escura, mesmo que o valor de carregados seja maior que a capacidade máxima.

### Integração com StackManager e Upgrades
- Toda a lógica de capacidade máxima, capacidade possível e quantidade carregada foi centralizada no `StackManager`.
- O `CapacityBarUI` tornou-se apenas um componente visual, recebendo os valores reais do StackManager.
- O `UpgradeManager` atualiza a barra imediatamente após upgrades, chamando `stackManager.UpdateCapacityBar()`.
- A UI reflete upgrades e carregamento em tempo real, sem duplicidade de dados.

### Boas práticas adotadas
- Separação clara entre lógica de jogo (StackManager) e visualização (CapacityBarUI).
- Atualização da UI sempre que a pilha muda ou upgrades são comprados.
- Código preparado para fácil expansão (animações, efeitos, ticks visuais, etc).

---

**Próximos passos sugeridos:**
- Adicionar animações suaves na barra.
- Implementar ticks visuais automáticos.
- Integrar efeitos visuais ao empilhar ou vender inimigos.
- Expandir upgrades e integração com outros sistemas de UI. 