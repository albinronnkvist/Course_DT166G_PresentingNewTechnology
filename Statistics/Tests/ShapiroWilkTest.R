# Example 1
data_normal <- rnorm(100, mean = 50, sd = 10)
shapiro.test(data_normal)

# Example 2
data_non_normal <- c(1, 2, 3, 4, 5, 6, 7, 8, 9, 100)
shapiro.test(data_non_normal)

data_small_normal <- c(48, 50, 52, 49, 51, 50)

# Perform Shapiro-Wilk test to check for normality
shapiro.test(data_small_normal)