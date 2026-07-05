## 1. Pense antes de escrever código

**Não faça suposições. Não esconda dúvidas. Apresente claramente os trade-offs.**

Antes de implementar:

* Explicite suas suposições. Se não tiver certeza, pergunte.
* Se houver mais de uma interpretação possível, apresente todas — não escolha uma sem avisar.
* Se houver uma solução mais simples, informe. Faça objeções quando necessário.
* Se algo estiver ambíguo, pare. Diga exatamente o que está confuso. Pergunte.


## 2. Priorize a simplicidade

**Código mínimo que resolve o problema. Nada especulativo.**

* Não adicione funcionalidades além do que foi pedido.
* Não crie abstrações para código de uso único.
* Não adicione "flexibilidade" ou "configurabilidade" que não foram solicitadas.
* Não implemente tratamento de erro para cenários impossíveis.
* Se você escrever 200 linhas e isso poderia ser feito em 50, reescreva.

Pergunte a si mesmo: "Um engenheiro sênior diria que isso está complicado demais?" Se sim, simplifique.


## 3. Alterações pontuais

**Altere apenas o que for necessário. Limpe apenas a bagunça que você mesmo criou.**

Ao editar código existente:

* Não "melhore" código, comentários ou formatação ao redor.
* Não adicione comentários no código ou em arquivos, a menos que isso seja explicitamente pedido.
* Não refatore o que não está quebrado.
* Siga o estilo existente, mesmo que você faria diferente.
* Se encontrar código obsoleto que não esteja relacionado, mencione — não remova.
* Remova imports, variáveis e funções que ficaram sem uso por causa das SUAS alterações.
* Não remova código obsoleto preexistente, a menos que isso seja pedido.
* Ao adicionar ou alterar mensagens, textos de validação, erros, labels ou textos exibidos ao usuário, verifique primeiro se a frase já existe nos arquivos de resource do projeto.
* Se a frase já existir, reutilize a chave existente em vez de criar uma nova.
* Se a frase ainda não existir, crie uma nova entrada no arquivo de resource apropriado e use essa chave no código.

O teste: cada linha alterada deve estar diretamente ligada ao pedido do usuário.

## 4. Execução orientada ao objetivo

**Defina os critérios de sucesso. Repita o processo até verificar que foram atendidos.**

Transforme tarefas em objetivos verificáveis:

* "Adicionar validação" → "Escrever testes para entradas inválidas e, depois, fazê-los passar"
* "Corrigir o bug" → "Escrever um teste que reproduza o bug e, depois, fazê-lo passar"
* "Refatorar X" → "Garantir que os testes passem antes e depois"

Para tarefas com múltiplas etapas, apresente um plano breve:

```text
1. [Etapa] → verificar: [checagem]
2. [Etapa] → verificar: [checagem]
3. [Etapa] → verificar: [checagem]
```

Critérios de sucesso fortes permitem que você avance de forma independente. Critérios fracos, como "fazer funcionar", exigem esclarecimentos constantes.

## 5. Controle de mudanças

**Trabalhe na branch atual. Não crie branches separadas.**

* Não crie uma nova branch, a menos que isso seja explicitamente pedido.
* Faça as alterações na branch atual para que eu possa revisar facilmente as diferenças no Visual Studio ou em outra ferramenta de comparação.
* Mantenha o diff pequeno, claro e diretamente relacionado ao pedido.