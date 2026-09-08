# Manual de Despliegue en Máquina Virtual (Azure)

Este documento detalla los comandos y procesos exactos utilizados para preparar el servidor Ubuntu 24.04 LTS en Azure y levantar la arquitectura de microservicios.

## 1. Conexión al Servidor (SSH)
Para acceder a la consola del servidor desde una terminal de Windows (PowerShell o CMD), nos autenticamos utilizando la clave privada RSA (`.pem`) generada por Azure.
```bash
ssh -i "C:\...\clave.pem" azureuser@57.151.129.118
```

## 2. Preparación del Entorno (Instalación de Dependencias)
Una vez dentro del servidor de Azure, se instalan las herramientas necesarias para descargar el código y orquestar los contenedores.
```bash
# Actualizar la lista de paquetes de Ubuntu
sudo apt update

# Instalar Git, Docker y Docker Compose
sudo apt install docker.io docker-compose git -y
```

## 3. Clonar el Repositorio de GitHub
Se descarga el código fuente del proyecto directamente en el servidor.
```bash
git clone [https://github.com/leamly/DIST-4AM-B3-AA01-DockerCompose-Azure-Paredes-Kevin.git](https://github.com/leamly/DIST-4AM-B3-AA01-DockerCompose-Azure-Paredes-Kevin.git)
```

## 4. Despliegue de la Arquitectura
Se ingresa al directorio del proyecto y se ejecuta el orquestador para construir las imágenes y levantar todos los servicios (Base de Datos, RabbitMQ, API Gateway y Microservicios).
```bash
# Entrar a la carpeta del proyecto clonado
cd DIST-4AM-B3-AA01-DockerCompose-Azure-Paredes-Kevin

# Levantar todos los contenedores en segundo plano (Modo Detached) y forzar la construcción (Build)
sudo docker-compose up -d --build
```
## 5. Verificación del Sistema
Para confirmar que los servicios, los puertos (80, 8081-8085, 15672) y las redes internas de Docker se levantaron correctamente:
```bash
sudo docker ps
```

## Comandos de Mantenimiento (En caso de reinicio)
```bash
# 1. Conectarse por SSH (Usando la nueva IP pública si cambió)
ssh -i "C:\...\clave.pem"azureuser@<NUEVA_IP_PUBLICA>

# 2. Entrar a la carpeta del proyecto
cd DIST-4AM-B3-AA01-DockerCompose-Azure-Paredes-Kevin

# 3. Descargar posibles cambios nuevos desde GitHub (Opcional)
git pull

# 4. Volver a encender los contenedores
sudo docker-compose up -d
```