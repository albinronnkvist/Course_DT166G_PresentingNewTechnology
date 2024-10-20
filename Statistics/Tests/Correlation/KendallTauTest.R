hours_studied <- c(1, 2, 3, 4, 6, 8, 30, 35, 37, 50)
exam_scores <- c(20, 25, 60, 65, 67, 72, 74, 71, 79, 80)

kendall_test <- cor.test(hours_studied, exam_scores, method = "kendall")

cat("Kendall's tau:", kendall_test$estimate, "\n")
cat("p-value:", kendall_test$p.value, "\n")