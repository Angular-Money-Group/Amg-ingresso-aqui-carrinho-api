git switch develop
git pull
docker container stop apicarrinho
docker container rm -f apicarrinho
docker build -t carrinho-api .
docker run -d -p 3003:80 --name apicarrinho carrinho-api