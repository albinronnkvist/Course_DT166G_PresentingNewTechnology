hours_studied <- c(1, 2, 3, 4, 6, 8, 30, 35, 37, 50)
exam_scores <- c(20, 25, 60, 65, 67, 80, 85, 75, 73, 80)

shapiro.test(hours_studied)
shapiro.test(exam_scores)

spearman_test <- cor.test(hours_studied, exam_scores, method = "spearman")

cat("Spearman's rho:", spearman_test$estimate, "\n")
cat("p-value:", spearman_test$p.value, "\n")