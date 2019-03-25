FROM microsoft/dotnet:sdk AS build-env

WORKDIR /Server

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore


FROM node as clientBuild
WORKDIR /Server
COPY . ./
RUN cd ClientApp && npm install && npm run build -- --prod

FROM build-env as publish

WORKDIR /Server
COPY --from=clientBuild /Server/ClientApp/dist ./ClientApp/dist
RUN dotnet publish -c Release -o build


# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /Server
COPY --from=build-env /build .
ENTRYPOINT ["dotnet", "sloflix.dll"]
