# Example 1: No significant difference (High p-value)
data1 <- c(50, 52, 54, 56, 58)
data2 <- c(51, 54, 57, 60, 63)
wilcox.test(data1, data2, paired = TRUE)

# Example 2: Significant difference (Low p-value)
data3 <- c(50, 52, 54, 56, 58, 60, 62, 64, 66, 68, 70, 72, 74, 76, 78)
data4 <- c(100, 102, 104, 106, 108, 110, 112, 114, 116, 118, 120, 122, 124, 126, 128)
wilcox.test(data3, data4, paired = TRUE)