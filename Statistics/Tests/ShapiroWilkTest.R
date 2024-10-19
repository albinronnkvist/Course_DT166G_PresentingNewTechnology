# Example 1
data_normal <- rnorm(100, mean = 50, sd = 10)
shapiro.test(data_normal)

# Example 2
data_non_normal <- c(1, 2, 3, 4, 5, 6, 7, 8, 9, 100)
shapiro.test(data_non_normal)