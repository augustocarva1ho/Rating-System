## Avaliação DescomplicaSP

Este é um projeto [Next.js](https://nextjs.org/) inicializado com [`create-next-app`](https://github.com/vercel/next.js/tree/canary/packages/create-next-app).

## Descrição do Projeto

O projeto Avaliacao-DescomplicaSP tem como objetivo principal coletar a avaliação dos usuários do serviço Descomplica após o atendimento. Os usuários podem expressar suas opiniões sobre o atendimento recebido utilizando uma escala de quatro opções de notas: Insatisfeito, Neutro, Satisfeito e Muito Satisfeito.

Ao selecionar uma dessas opções de avaliação, o sistema registra a nota fornecida pelo usuário juntamente com a unidade de atendimento onde a avaliação foi realizada. Esses dados são então enviados para uma API, permitindo que a administração do Descomplica possa analisar as avaliações e identificar áreas que necessitam de melhorias ou que estão desempenhando bem.

Este sistema visa proporcionar um meio eficiente e estruturado para os cidadãos de São Paulo darem seu feedback sobre o serviço público, contribuindo para a melhoria contínua do atendimento oferecido pelo Descomplica.

## Começando

Primeiro, inicie o servidor de desenvolvimento:

```bash
npm run dev
# ou
yarn dev
# ou
pnpm dev
# ou
bun dev
```

Depois, acesse \src\pages\api e inicie o servidor da API:

```bash
node index.js
```

Abra http://localhost:3000 no seu navegador para ver o resultado.

Você pode começar a editar a página modificando pages/index.tsx. A página será atualizada automaticamente à medida que você edita o arquivo.

Dados da API podem ser acessados em http://localhost:5000/dados-recebidos. Este endpoint pode ser editado em pages/api/index.js.

O diretório pages/api é mapeado para /api/*. Arquivos neste diretório são tratados como roteiros de API em vez de páginas React.

Este projeto utiliza next/font para otimizar e carregar automaticamente o Inter, uma fonte do Google personalizada.

## Estrutura

A página principal se encontra no arquivo pages/index.tsx.

O principal elemento do sistema, a avaliação, se encontra em components/Avaliacao.tsx.

O código da api está na pasta pages/api. 


## Instalações

```bash
npm install react-icons react-input-mask react-intersection-observer next-router cors express 
#ou
yarn add react-icons react-input-mask react-intersection-observer next-router cors express
#ou
pnpm add react-icons react-input-mask react-intersection-observer next-router cors express
```
## Storybook

Este projeto utiliza o [Storybook](https://storybook.js.org/) para desenvolver componentes isoladamente.

### Instalação

Para instalar o Storybook, execute o seguinte comando no diretório do projeto:

```bash
npx sb init
```

## Rotas da API

As rotas da API disponíveis são:

http://localhost:5000/enviar > envia os dados para a API com POST
http://localhost:5000/dados-recebidos > página que mostra os dados recebidos com GET

## Deploy com Vercel

A maneira mais fácil de implantar seu aplicativo Next.js é usar a Plataforma Vercel(https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme), dos criadores do Next.js.

Confira a documentação sobre implantação do Next.js(https://nextjs.org/docs/deployment) para mais detalhes.
