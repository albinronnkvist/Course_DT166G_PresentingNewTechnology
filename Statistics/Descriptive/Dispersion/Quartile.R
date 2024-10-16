data <- c(1, 2, 3, 4, 5, 6, 7, 8, 9)

quartiles <- quantile(data, type = 2)
Q1 <- quantile(data, 0.25)
Q3 <- quantile(data, 0.75)

quartiles
Q1
Q3