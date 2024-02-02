git switch develop
git pull
docker container stop apicarrinho
docker container rm -f apicarrinho
docker build -t carrinho-api .
docker run -d -p 3003:80 --name apicarrinho carrinho-api
docker run -e DbMongoConcectionString="mongodb+srv://angularmoneygroup:5139bpOk9VR1GeI7@ingressosaqui.t49bdes.mongodb.net/ingressosAqui?retryWrites=true&w=majority" , DatabaseName="ingressosAquiHomolog" -d -p 3003:80 --name apicarrinho carrinho-api