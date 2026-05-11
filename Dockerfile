FROM ubuntu:22.04

ENV DEBIAN_FRONTEND=noninteractive

# Cài thêm openssh-server để Web có thể SSH vào ra lệnh
RUN apt-get update && apt-get install -y \
    ansible \
    python3-pip \
    openssh-server \
    sshpass \
    && rm -rf /var/lib/apt/lists/*

RUN pip3 install "pywinrm>=0.3.0"

# Cấu hình SSH để cho phép đăng nhập root (dùng cho việc Web điều khiển)
RUN mkdir /var/run/sshd
RUN echo 'root:123' | chpasswd 
RUN sed -i 's/#PermitRootLogin prohibit-password/PermitRootLogin yes/' /etc/ssh/sshd_config

WORKDIR /data

# Mở port 22 cho SSH
EXPOSE 22

# Khởi động SSH server khi container chạy
CMD ["/usr/sbin/sshd", "-D"]