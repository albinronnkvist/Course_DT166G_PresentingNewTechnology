# Example 1: Data with no significant difference (High p-value)
data1 <- c(50, 60, 55, 65, 70)
data2 <- c(51, 61, 54, 66, 71)
t.test(data1, data2, paired = TRUE)

# Example 2: Data with significant difference (Low p-value)
data3 <- c(50, 60, 55, 65, 70)
data4 <- c(70, 75, 80, 85, 90)
t.test(data3, data4, paired = TRUE)